using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _tbx_UtilityClass : _tbx_BaseClass
{
    [Header("Habilidad1")]
    // Reference to the prefab for the ability object
    public GameObject prefabHabilidad1;
    // Maximum number of ability objects that can be alive at the same time
    public int cantidadMaxHabilidad1;
    // Current number of ability objects that are alive
    private int cantidadHabilidad1Actual = 0;
    // Speed of the ability object
    public float habilidad1ObjectSpeed;
    // Direction of the ability object
    public Vector3 habilidad1ObjectDirection = Vector3.forward;

    // Reference to the player object
    public GameObject player;

    public override void Start()
    {
        base.Start();

        // Find the player object in the scene
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Spawn a new ability object
    public override void Habilidad1()
    {
        // Check if we can spawn a new ability object
        if (cantidadHabilidad1Actual < cantidadMaxHabilidad1)
        {
            // Spawn a new ability object at the player's position
            Instantiate(prefabHabilidad1, player.transform.position, Quaternion.identity);

            // Increment the current number of ability objects
            cantidadHabilidad1Actual++;
        }
        else
        {
            Debug.Log("Maximum number of ability objects reached!");
        }
    }

    // Perform the second ability
    public override void Habilidad2()
    {
        Debug.Log("Habilidad 2 - Utility");
    }

    // Perform the third ability
    public override void Habilidad3()
    {
        Debug.Log("Habilidad 3 - Utility");
    }
}