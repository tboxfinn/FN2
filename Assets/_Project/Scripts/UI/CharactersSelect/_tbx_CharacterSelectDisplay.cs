using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class _tbx_CharacterSelectDisplay : NetworkBehaviour
{
    [SerializeField] private _tbx_CharacterDataBase characterDataBase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private _tbx_CharacterSelectButton selectButtonPrefab;
    [SerializeField] private _tbx_PlayerCard[] playerCards;
    [SerializeField] private GameObject characterInfoPanel;
    [SerializeField] private TMP_Text characterNameText;

    private NetworkList<_tbx_CharacterSelectState> players;

    private void Awake()
    {
        players = new NetworkList<_tbx_CharacterSelectState>();
    }

    override public void OnNetworkSpawn()
    {
        if (IsClient)
        {
            _tbx_Character[] allCharacters = characterDataBase.GetAllCharacters();

            foreach (var character in allCharacters)
            {
                var selectButtonInstance = Instantiate(selectButtonPrefab, charactersHolder);
                selectButtonInstance.SetCharacter(this, character);
            }

            players.OnListChanged += HandlePlayersStateChanged;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if(IsClient)
        {
            players.OnListChanged -= HandlePlayersStateChanged;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected to server");

        players.Add(new _tbx_CharacterSelectState(clientId));
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} disconnected from server");

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == clientId)
            {
                players.RemoveAt(i);
                break;
            }
        }
    }

    public void Select(_tbx_Character character)
    {
        characterNameText.text = character.DisplayName;

        characterInfoPanel.SetActive(true);

        SelectServerRpc(character.Id);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == serverRpcParams.Receive.SenderClientId)
            {
                players[i] = new _tbx_CharacterSelectState(players[i].ClientId, characterId);
                break;
            }
        }
    }

    private void HandlePlayersStateChanged(NetworkListEvent<_tbx_CharacterSelectState> changeEvent)
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if(players.Count > i)
            {
                playerCards[i].UpdateDisplay(players[i]);
            }
            else
            {
                playerCards[i].DisableDsplay();
            }
        }
    }
}
