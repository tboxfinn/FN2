using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class _alx_NetObjectPolling : NetworkBehaviour
{
    [SerializeField] private GameObject prefab; // El prefab de la bala que quieres agrupar.
    [SerializeField] private int size; // El tamaño inicial del grupo.

    private Queue<NetworkObject> pool; // La cola que contiene las balas agrupadas.

    // Este método se llama cuando se carga la escena.
    void Awake()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab no asignado en el Inspector de Unity.");
            return;
        }

        pool = new Queue<NetworkObject>(); // Inicializa la cola.

        // Crea las balas y las añade a la cola.
        for (int i = 0; i < size; i++)
        {
            NetworkObject networkObject = Instantiate(prefab).GetComponent<NetworkObject>();
            networkObject.gameObject.SetActive(false); // Desactiva la bala.
            pool.Enqueue(networkObject); // Añade la bala a la cola.
        }
    }

    // Este método se utiliza para obtener una bala del grupo.
    public NetworkObject Get()
    {
        if (pool != null && pool.Count > 0)
        {
            NetworkObject networkObject = pool.Dequeue(); // Obtiene la primera bala de la cola.
            networkObject.gameObject.SetActive(true); // Activa la bala.

            return networkObject; // Devuelve la bala.
        }
        else
        {
            Debug.LogWarning("La cola está vacía o no se ha inicializado.");
            return null;
        }
    }

    // Este método se utiliza para devolver una bala al grupo.
    public void Return(NetworkObject networkObject)
    {
        if (networkObject != null)
        {
            networkObject.gameObject.SetActive(false); // Desactiva la bala.
            pool.Enqueue(networkObject); // Añade la bala de nuevo a la cola.
        }
        else
        {
            Debug.LogWarning("Intentando devolver un objeto nulo al grupo.");
        }
    }
}
