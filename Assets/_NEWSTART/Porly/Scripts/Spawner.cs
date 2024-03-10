using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objetoPrefab;
    public Transform[] posicionesSpawn;

    public float spawnTime = 3f;

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, spawnTime);
    }

    void SpawnObject()
    {
        if (objetoPrefab != null && posicionesSpawn.Length > 0)
        {
            int indicePosicion = Random.Range(0, posicionesSpawn.Length);
            Transform posicionSpawn = posicionesSpawn[indicePosicion];

            Instantiate(objetoPrefab, posicionSpawn.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Prefab no asignado o posiciones de spawn no configuradas en el spawner.");
        }
    }
}
