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
    //public List<GameObject> habilidad1Objects = new List<GameObject>();
    //public GameObject prefabHabilidad1;
    // Maximum number of ability objects that can be alive at the same time
   // public int cantidadMaxHabilidad1;
    // Current number of ability objects that are alive
    //private int cantidadHabilidad1Actual = 0;
    public Transform PrefabBalaStun;
    public bool Hab1Selected;
    public float tiempoHabilidad1;

    [Header("Habilidad2")]
    public float velocidadDeTiroInicial;
    public float tiempoHabilidad2;

    [Header("Habilidad3")]
    public GameObject prefabHabilidad3;
    public float tiempoHabilidad3;
    

    [Header("References")]
    // Reference to the player object
    public GameObject player;
    public GameObject playerObj;

    public override void Start()
    {
        base.Start();
        gunData.fireRate = 300;
        // Find the player object in the scene
        player = GameObject.FindGameObjectWithTag("Player");
        velocidadDeTiroInicial = gunData.fireRate;
        
    }

    // Spawn a new ability object
    public override void Habilidad1()
    {
        Debug.Log("Utility - BearTrap");
        // Check if we can spawn a new ability object
        StartCoroutine(StunBullets());
        
        /*if (cantidadHabilidad1Actual < cantidadMaxHabilidad1)
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
        }*/
    }

    private IEnumerator StunBullets()
    {
        
        gunData.currentAmmo = 1;
        //gunData.fireRate = 10f;
        Hab1Selected = true;

        yield return new WaitForSeconds(gunData.reloadTime-0.1f);

        Hab1Selected = false;
        gunData.fireRate = velocidadDeTiroInicial;
        
    }

    // Perform the second ability
    public override void Habilidad2()
    {
        Debug.Log("Habilidad 2 - AumentoVelDisparo");
        StartCoroutine(ChangeFirerate());
        cancelReloadInput?.Invoke();
    }

    private IEnumerator ChangeFirerate()
    {
        gunData.fireRate *= 2f;
        yield return new WaitForSeconds(tiempoHabilidad2);
        gunData.fireRate = velocidadDeTiroInicial;
    }

    // Perform the third ability
    public override void Habilidad3()
    {
        Debug.Log("Habilidad 3 - Utility");
        StartCoroutine(EscudoGiratorio());
    }

    private IEnumerator EscudoGiratorio()
    {
        GameObject newHabilidad3Object = Instantiate(prefabHabilidad3, playerObj.transform.position, Quaternion.identity);
        newHabilidad3Object.transform.parent = playerObj.transform;
        yield return new WaitForSeconds(tiempoHabilidad3);
        Destroy(newHabilidad3Object);
    }

    public override void Shoot()
    {
        if (Hab1Selected && gunData.currentAmmo == 1)
        {
            Debug.Log("Disparo2");

            Vector3 aimDir = mouseWorldPosition - spawnBulletPosition.position;
            Instantiate(PrefabBalaStun, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
        else
        {
            Debug.Log("Disparo1");

            Vector3 aimDir = mouseWorldPosition - spawnBulletPosition.position;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }

        
    }
}