using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SmokeBomb : MonoBehaviour
{
    public float lifeTime = 5f; // Define el tiempo de vida del objeto

    // Start is called before the first frame update
    void Start()
    {
        // Destruye el objeto después de un cierto tiempo
        Destroy(gameObject, lifeTime);
    }
}