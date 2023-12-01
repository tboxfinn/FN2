using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.TextCore.Text;

public class _tbx_CharacterSpawner : NetworkBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("References")]
    [SerializeField] private _tbx_CharacterDataBase characterDataBase;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        foreach (var client in _tbx_HostManager.Instance.ClientData)
        {
            var character = characterDataBase.GetCharacterById(client.Value.characterId);
            if (character != null)
            {
                // Select a random spawn point
                var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

                //var characterInstance = Instantiate(character.GameplayPrefab, spawnPoint.transform.position, Quaternion.identity);
                //Instantiate(character.GameplayPrefab, client.Value.spawnPoint.position, Quaternion.identity);
                
                //var spawnPos = new Vector3(Random.Range(-5, 5), 3, Random.Range(-5, 5));
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPoint.position, Quaternion.identity);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
            }
        }
    }
}

