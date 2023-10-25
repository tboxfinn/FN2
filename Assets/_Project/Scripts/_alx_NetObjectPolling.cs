using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectPooling : NetworkBehaviour
{
    [SerializeField] private GameObject prefab; // El prefab de la bala que quieres agrupar.
    [SerializeField] private int size; // El tama�o inicial del grupo.

    private Queue<NetworkObject> pool; // La cola que contiene las balas agrupadas.
    private bool isInitialized = false;

    // Este m�todo se llama al inicio del juego.
    void Start()
    {
        if (IsServer && !isInitialized)
        {
            InitializePool();
        }
    }

    // Este m�todo se utiliza para inicializar el grupo de balas.
    private void InitializePool()
    {
        pool = new Queue<NetworkObject>(); // Inicializa la cola.

        // Crea las balas y las a�ade a la cola.
        for (int i = 0; i < size; i++)
        {
            NetworkObject networkObject = Instantiate(prefab).GetComponent<NetworkObject>();
            networkObject.gameObject.SetActive(false); // Desactiva la bala.
            pool.Enqueue(networkObject); // A�ade la bala a la cola.
        }

        isInitialized = true;
    }

    // Este m�todo se utiliza para obtener una bala del grupo.
    public NetworkObject Get()
    {
        if (IsServer)
        {
            if (!isInitialized)
            {
                InitializePool();
            }

            if (pool.Count == 0)
            {
                return null; // Si el grupo est� vac�o, devuelve null.
            }

            NetworkObject networkObject = pool.Dequeue(); // Obtiene la primera bala de la cola.
            networkObject.gameObject.SetActive(true); // Activa la bala.

            return networkObject; // Devuelve la bala.
        }
        else
        {
            return null;
        }
    }

    // Este m�todo se utiliza para devolver una bala al grupo.
    public void Return(NetworkObject networkObject)
    {
        if (IsServer)
        {
            networkObject.gameObject.SetActive(false); // Desactiva la bala.
            pool.Enqueue(networkObject); // A�ade la bala de nuevo a la cola.
        }
    }
}
