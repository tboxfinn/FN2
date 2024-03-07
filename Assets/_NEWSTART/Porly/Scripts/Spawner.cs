using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objetoPrefab;
    public Transform[] posicionesSpawn;

    void Start()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        int indicePosicion = Random.Range(0, posicionesSpawn.Length);
        Transform posicionSpawn = posicionesSpawn[indicePosicion];

        Instantiate(objetoPrefab, posicionSpawn.position, Quaternion.identity);
    }
}
