using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class _alx_NetObjectPolling : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int size;

    private Queue<NetworkObject> pool;

    void Start()
    {
        pool = new Queue<NetworkObject>();

        for (int i = 0; i < size; i++)
        {
            NetworkObject networkObject = Instantiate(prefab).GetComponent<NetworkObject>();
            networkObject.gameObject.SetActive(false);
            pool.Enqueue(networkObject);
        }
    }

    public NetworkObject Get()
    {
        if (pool.Count == 0)
        {
            return null;
        }

        NetworkObject networkObject = pool.Dequeue();
        networkObject.gameObject.SetActive(true);

        return networkObject;
    }

    public void Return(NetworkObject networkObject)
    {
        networkObject.gameObject.SetActive(false);
        pool.Enqueue(networkObject);
    }
}

public interface INetworkPrefabInstanceHandler
{
    NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation);
    void Destroy(NetworkObject networkObject);
}
