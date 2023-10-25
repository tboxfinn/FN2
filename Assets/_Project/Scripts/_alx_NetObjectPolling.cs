using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Esta es la clase principal que maneja el agrupamiento de objetos de red.
public class _alx_NetObjectPolling : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // El prefab que quieres agrupar.
    [SerializeField] private int size; // El tamaño inicial del grupo.

    private Queue<NetworkObject> pool; // La cola que contiene los objetos agrupados.

    // Este método se llama al inicio del juego.
    void Start()
    {
        pool = new Queue<NetworkObject>(); // Inicializa la cola.

        // Crea los objetos y los añade a la cola.
        for (int i = 0; i < size; i++)
        {
            NetworkObject networkObject = Instantiate(prefab).GetComponent<NetworkObject>();
            networkObject.gameObject.SetActive(false);
            pool.Enqueue(networkObject);
        }
    }

    // Este método se utiliza para obtener un objeto del grupo.
    public NetworkObject Get()
    {
        if (pool.Count == 0)
        {
            return null; // Si el grupo está vacío, devuelve null.
        }

        NetworkObject networkObject = pool.Dequeue(); // Obtiene el primer objeto de la cola.
        networkObject.gameObject.SetActive(true); // Activa el objeto.

        return networkObject; // Devuelve el objeto.
    }

    // Este método se utiliza para devolver un objeto al grupo.
    public void Return(NetworkObject networkObject)
    {
        networkObject.gameObject.SetActive(false); // Desactiva el objeto.
        pool.Enqueue(networkObject); // Añade el objeto de nuevo a la cola.
    }
}

// Esta interfaz se utiliza para personalizar cómo se instancian y destruyen los objetos de red.
public interface INetworkPrefabInstanceHandler
{
    NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation);
    void Destroy(NetworkObject networkObject);
}

