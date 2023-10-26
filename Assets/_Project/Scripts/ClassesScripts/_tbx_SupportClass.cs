using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _tbx_SupportClass : _tbx_BaseClass
{
    public _tbx_PlayerMovementScript playerMovementScript;

    public float effectDuration;

    [SerializeField] private float initialMoveSpeed;

    public float fuerzaDeTiroH2;
    public float distanciaSpawnHabilidad2;
    public List<GameObject> habilidad2Objects = new List<GameObject>();
    public GameObject prefabHabilidad2;
    public int cantidadMaxHabilidad2;
    private int cantidadHabilidad2Actual = 0;
    public GameObject player;
    public GameObject playerObj;

    public float fuerzaDeTiroH1;
    public float distanciaSpawnHabilidad1;
    public List<GameObject> habilidad1Objects = new List<GameObject>();
    public GameObject prefabHabilidad1;
    public int cantidadMaxHabilidad1;
    private int cantidadHabilidad1Actual = 0;

    public override void Start()
    {
        base.Start();
        
        if (_ply_PlayerHealth.instance != null)
        {
            _ply_PlayerHealth.instance.OnHealthChanged += HandleHealthChanged;
        }
        else
        {
            Debug.LogError("El objeto encontrado no tiene el componente _ply_PlayerHealth");
        }

        //Save the initial value of moveSpeed
        initialMoveSpeed = playerMovementScript.moveSpeed;
        //Set the bullets to the magazine size
        actualBullets = magazineSize;
    }

    private new void OnDestroy()
    {
        if (_ply_PlayerHealth.instance != null)
        {
            _ply_PlayerHealth.instance.OnHealthChanged -= HandleHealthChanged;
        }
    }

    public override void Habilidad1()
    {
        Debug.Log("Habilidad 1- Support");
        if (cantidadHabilidad1Actual < cantidadMaxHabilidad1)
        {
            // Spawn a new ability object in front of the player
            GameObject newHabilidad1Object = Instantiate(prefabHabilidad1, playerObj.transform.position + playerObj.transform.forward * distanciaSpawnHabilidad1, Quaternion.identity);
            Rigidbody newHabilidad1ObjectRigidbody = newHabilidad1Object.GetComponent<Rigidbody>();

            // Apply force to the ability object
            newHabilidad1ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiroH1, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad1Objects.Add(newHabilidad1Object);

            // Increment the current number of ability objects
            cantidadHabilidad1Actual++;
            Destroy(newHabilidad1Object, 5f);
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
            newHabilidad1ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiroH1, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad2Objects.Add(newHabilidad1Object);
        }
    }

    public override void Habilidad2()
    {
        Debug.Log("Habilidad 2- Support");
        if (cantidadHabilidad2Actual < cantidadMaxHabilidad2)
        {
            // Spawn a new ability object in front of the player
            GameObject newHabilidad2Object = Instantiate(prefabHabilidad2, playerObj.transform.position + playerObj.transform.forward * distanciaSpawnHabilidad2, Quaternion.identity);
            Rigidbody newHabilidad1ObjectRigidbody = newHabilidad2Object.GetComponent<Rigidbody>();

            // Apply force to the ability object
            newHabilidad1ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiroH2, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad2Objects.Add(newHabilidad2Object);

            // Increment the current number of ability objects
            cantidadHabilidad2Actual++;
            Destroy(newHabilidad2Object, 5f);
        }
        else
        {
            // Destroy the first spawned object of this type and remove it from the list
            GameObject firstHabilidad2Object = habilidad2Objects[0];
            habilidad2Objects.RemoveAt(0);
            Destroy(firstHabilidad2Object);

            // Spawn a new ability object in front of the player
            GameObject newHabilidad2Object = Instantiate(prefabHabilidad2, playerObj.transform.position + playerObj.transform.forward * distanciaSpawnHabilidad2, Quaternion.identity);
            Rigidbody newHabilidad2ObjectRigidbody = newHabilidad2Object.GetComponent<Rigidbody>();

            // Apply force to the ability object
            newHabilidad2ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiroH2, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad2Objects.Add(newHabilidad2Object);
        }
    }

    public override void Habilidad3()
    {
        Debug.Log("Habilidad 3- Support");

        Debug.Log("Reduce speed");
        // Reduce player movement speed
        playerMovementScript.moveSpeed = playerMovementScript.moveSpeed - 3;
        // Start a coroutine to reset the values after a delay
        StartCoroutine(ResetMovementValues());

        if (_ply_PlayerHealth.instance != null)
        {
            _ply_PlayerHealth.instance.IncreaseHealth();
        }

    }

    public override void Shoot()
    {
        Debug.Log("Disparo3");

        Vector3 aimDir = mouseWorldPosition - spawnBulletPosition.position;
        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }

    private void HandleHealthChanged(int newHealth, Vector3 newScale)
    {
        Debug.Log("Vida actual: " + newHealth);
        Debug.Log("Nueva escala: " + newScale);
    }

    private IEnumerator ResetMovementValues()
    {
        yield return new WaitForSeconds(effectDuration);

        // Reset the moveSpeed property of the playerMovementScript
        playerMovementScript.moveSpeed = initialMoveSpeed;
    }
}
