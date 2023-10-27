using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _tbx_UtilityClass : _tbx_BaseClass
{
    [Header("Utility Class")]
    public float fuerzaDeTiro;
    public float distanciaSpawnHabilidad1;

    [Header("Habilidad1")]
    // Reference to the prefab for the ability object
    public List<GameObject> habilidad1Objects = new List<GameObject>();
    public GameObject prefabHabilidad1;
    // Maximum number of ability objects that can be alive at the same time
    public int cantidadMaxHabilidad1;
    // Current number of ability objects that are alive
    private int cantidadHabilidad1Actual = 0;

    [Header("Habilidad2")]
    public float velocidadDeTiroInicial;
    public float tiempoHabilidad2;
    

    [Header("References")]
    // Reference to the player object
    public GameObject player;
    public GameObject playerObj;
    public GunData gunData;

    public override void Start()
    {
        base.Start();

        // Find the player object in the scene
        player = GameObject.FindGameObjectWithTag("Player");
        velocidadDeTiroInicial = gunData.fireRate;
    }

    // Spawn a new ability object
    public override void Habilidad1()
    {
        Debug.Log("Utility - BearTrap");
        // Check if we can spawn a new ability object
        if (cantidadHabilidad1Actual < cantidadMaxHabilidad1)
        {
            // Spawn a new ability object in front of the player
            GameObject newHabilidad1Object = Instantiate(prefabHabilidad1, playerObj.transform.position + playerObj.transform.forward * distanciaSpawnHabilidad1, Quaternion.identity);
            Rigidbody newHabilidad1ObjectRigidbody = newHabilidad1Object.GetComponent<Rigidbody>();
            
            // Apply force to the ability object
            newHabilidad1ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiro, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad1Objects.Add(newHabilidad1Object);

            // Increment the current number of ability objects
            cantidadHabilidad1Actual++;
        }
        else
        {
            // Destroy the first spawned object of this type and remove it from the list
            GameObject firstHabilidad1Object = habilidad1Objects[0];
            habilidad1Objects.RemoveAt(0);
            Destroy(firstHabilidad1Object);

            // Spawn a new ability object in front of the player
            GameObject newHabilidad1Object = Instantiate(prefabHabilidad1, playerObj.transform.position + playerObj.transform.forward * distanciaSpawnHabilidad1, Quaternion.identity);
            Rigidbody newHabilidad1ObjectRigidbody = newHabilidad1Object.GetComponent<Rigidbody>();
            
            // Apply force to the ability object
            newHabilidad1ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiro, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad1Objects.Add(newHabilidad1Object);
        }
    }

    // Perform the second ability
    public override void Habilidad2()
    {
        Debug.Log("Habilidad 2 - Utility");
        StartCoroutine(ChangeFirerate());
    }

    private IEnumerator ChangeFirerate()
    {
        gunData.fireRate = gunData.fireRate * 1.5f;
        yield return new WaitForSeconds(tiempoHabilidad2);
        gunData.fireRate = velocidadDeTiroInicial;
    }

    // Perform the third ability
    public override void Habilidad3()
    {
        Debug.Log("Habilidad 3 - Utility");
    }
    public override void Shoot()
    {
        Debug.Log("Disparo2");

        Vector3 aimDir = mouseWorldPosition - spawnBulletPosition.position;
        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
}