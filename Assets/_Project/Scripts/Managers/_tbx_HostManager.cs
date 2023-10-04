using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class _tbx_HostManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxConnections = 6;
    [SerializeField] private string characterSelectionSceneName = string.Empty;
    [SerializeField] private string gameplaySceneName = string.Empty;

    public static _tbx_HostManager Instance {get; private set;}

    public Dictionary<ulong, _tbx_ClientData> ClientData {get; private set;}
    public string JoinCode {get; private set;}

    private bool gameHasStarted;

    private string lobbyId;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public async void StartHost()
    {
        Allocation allocation;

        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        }
        catch(Exception e)
        {
            Debug.Log($"Failed to create allocation: {e.Message}");
            throw;
        }

        Debug.Log($"Server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"Server: {allocation.AllocationId}");

        try 
        {
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }
        catch
        {
            Debug.Log($"Failed to get join code");
            throw;
        }

        var relayServerData = new RelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        try
        {
            var createLobbyOptions = new CreateLobbyOptions();
            createLobbyOptions.IsPrivate = false;
            createLobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "joinCode", new DataObject(visibility: DataObject.VisibilityOptions.Member, value: JoinCode)
                    
                }
            };

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("My Lobby", maxConnections,createLobbyOptions);
            lobbyId = lobby.Id;
            StartCoroutine(HeartbeatLobbyCoroutine(15f));
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"Failed to create lobby: {e.Message}");
            throw;
        }

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

        ClientData = new Dictionary<ulong, _tbx_ClientData>();

        NetworkManager.Singleton.StartHost();
    }

    private IEnumerator HeartbeatLobbyCoroutine(float waitTimeSeconds)
    {
        var delay = new WaitForSeconds(waitTimeSeconds);
        while(true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    public void StartServer()
    {
        //aqui podemos poner acciones que se ejecuten cuando se inicie el servidor
        //por ejemplo, cargar el mapa, definir equipos, etc
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

        ClientData = new Dictionary<ulong, _tbx_ClientData>();

        NetworkManager.Singleton.StartServer();
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if(ClientData.Count>=3 || gameHasStarted)
        {
            response.Approved = false;
            return;
        }

        response.Approved = true;
        response.CreatePlayerObject = false;
        response.Pending = false;

        ClientData[request.ClientNetworkId] = new _tbx_ClientData(request.ClientNetworkId);
        
        Debug.Log($"Added client with id {request.ClientNetworkId} to the server");
    }

    private void OnNetworkReady()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        NetworkManager.Singleton.SceneManager.LoadScene(characterSelectionSceneName, LoadSceneMode.Single);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if(ClientData.ContainsKey(clientId))
        {
            if(ClientData.Remove(clientId))
            {
                Debug.Log($"Removed client with id {clientId} from the server");
            }
        }
    }

    public void SetCharacter(ulong clientId, int characterId)
    {
        if(ClientData.TryGetValue(clientId, out _tbx_ClientData data))
        {
            data.characterId = characterId;
        }
    }

    public void StartGame()
    {
        gameHasStarted = true;

        NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }

}
