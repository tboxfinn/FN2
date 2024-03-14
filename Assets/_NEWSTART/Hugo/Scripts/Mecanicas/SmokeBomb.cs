using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SmokeBomb : NetworkBehaviour
{
    public GameObject smokePrefab;

    void ExplodeSmokeBomb()
    {
        // Instanciar el humo en la posición de la bomba
        GameObject smokeInstance = Instantiate(smokePrefab, transform.position, Quaternion.identity);

        // Destruir el humo después de un tiempo
        Destroy(smokeInstance, 5f); // Ajusta el tiempo según tus necesidades
    }

    void Update()
    {
        if (IsServer)
        {
            // Lógica para detectar cuando la bomba de humo explota
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Sincronizar la explosión en todos los clientes
                ExplodeSmokeBombClientRpc();
            }
        }
    }

    [ClientRpc]
    void ExplodeSmokeBombClientRpc()
    {
        // Llamar a la función de explosión en todos los clientes
        ExplodeSmokeBomb();
    }
}
