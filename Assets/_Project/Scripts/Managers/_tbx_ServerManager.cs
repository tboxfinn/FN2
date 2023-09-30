using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class _tbx_ServerManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string characterSelectionSceneName = string.Empty;
    [SerializeField] private string gameplaySceneName = string.Empty;

    public static _tbx_ServerManager Instance {get; private set;}

    public Dictionary<ulong, _tbx_ClientData> ClientData {get; private set;}

    private bool gameHasStarted;

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

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

        ClientData = new Dictionary<ulong, _tbx_ClientData>();

        NetworkManager.Singleton.StartHost();
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
