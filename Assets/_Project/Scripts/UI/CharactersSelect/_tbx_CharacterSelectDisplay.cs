using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class _tbx_CharacterSelectDisplay : NetworkBehaviour
{
    [SerializeField] private _tbx_CharacterDataBase characterDataBase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private _tbx_CharacterSelectButton selectButtonPrefab;
    [SerializeField] private _tbx_PlayerCard[] playerCards;
    [SerializeField] private GameObject characterInfoPanel;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Transform introSpawnPoint;
    [SerializeField] private Button lockInButton;

    private GameObject introInstance;
    private List<_tbx_CharacterSelectButton> characterButtons = new List<_tbx_CharacterSelectButton>();

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
                characterButtons.Add(selectButtonInstance);
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
        for (int i = 0; i < players.Count; i++)
        {
            if(players[i].ClientId != NetworkManager.Singleton.LocalClientId)
            {
                continue;
            }

            //Check if the player is already locked in
            if (players[i].IsLockedIn)
            {
                return;
            }

            //Check if we are not selecting the same character
            if (players[i].CharacterId == character.Id)
            {
                return;
            }

            //Check if the character is already taken
            if (IsCharacterTaken(character.Id, false))
            {
                return;
            }

        }

        characterNameText.text = character.DisplayName;

        characterInfoPanel.SetActive(true);

        if (introInstance != null)
        {
            Destroy(introInstance);
        }

        introInstance = Instantiate(character.IntroPrefab, introSpawnPoint);

        SelectServerRpc(character.Id);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }

            if (!characterDataBase.IsValidCharacterId(characterId)) { return; }

            if (IsCharacterTaken(characterId, true)) { return; }
            
            players[i] = new _tbx_CharacterSelectState(players[i].ClientId, characterId,  players[i].IsLockedIn);
            
        }
    }

    public void LockIn()
    {
        LockInServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void LockInServerRpc(ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }

            if (!characterDataBase.IsValidCharacterId(players[i].CharacterId)) { return; }

            if (IsCharacterTaken(players[i].CharacterId, true)) { return; }
            
            players[i] = new _tbx_CharacterSelectState(players[i].ClientId, players[i].CharacterId,  true);
            
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

        foreach (var button in characterButtons)
        {
            if (button.IsDisabled)
            {
                continue;
            }

            if (IsCharacterTaken(button.Character.Id, false))
            {
                button.SetDisabled();
            }
        }

        foreach (var player in players)
        {
            if (player.ClientId != NetworkManager.Singleton.LocalClientId)
            {
                continue;
            }

            if (player.IsLockedIn)
            {
                lockInButton.interactable = false;
                break;
            }
            
            if (IsCharacterTaken(player.CharacterId, false))
            {
                lockInButton.interactable = false;
                break;
            }

            lockInButton.interactable = true;

            break;
        }
    }

    private bool IsCharacterTaken(int characterId, bool checkAll)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (!checkAll)
            {
                if (players[i].ClientId == NetworkManager.Singleton.LocalClientId)
                {
                    continue;
                }
            }

            if (players[i].IsLockedIn && players[i].CharacterId == characterId)
            {
                return true;
            }
        }

        return false;
    }
}
