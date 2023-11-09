using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    public _ply_PlayerHealth playerHealth;

    public bool ClientID { get; private set; }

    public override void Start()
    {
        if (!IsLocalPlayer) return;

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

        //Stats
        health = maxHealth;
        
        Hab1 = KeyCode.Alpha1;
        Hab2 = KeyCode.Alpha2;
        Hab3 = KeyCode.Alpha3;
        ReloadKey = KeyCode.R;
        CancelReloadKey = KeyCode.T;
        Debug.Log("Base Class");

        //Habilidades
        imageHab1.fillAmount = 0;
        imageHab2.fillAmount = 0;
        imageHab3.fillAmount = 0;

        textHab1.text = "";
        textHab2.text = "";
        textHab3.text = "";
    }

    /*public void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        //ActionInput
        if (Input.GetKeyDown(ActionKey))
        {
            MakeAction();
        }

        //Hability1Input
        if (Input.GetKeyDown(Hab1) && !isHab1OnCooldown)
        {
            isHab1OnCooldown = true;
            currentCooldownHab1 = cooldownHab1;
            Habilidad1();
        }

        //Hability2Input
        if (Input.GetKeyDown(Hab2) && !isHab2OnCooldown)
        {
            isHab2OnCooldown = true;
            currentCooldownHab2 = cooldownHab2;
            Habilidad2();
        }

        //Hability3Input
        if (Input.GetKeyDown(Hab3) && !isHab3OnCooldown)
        {
            isHab3OnCooldown = true;
            currentCooldownHab3 = cooldownHab3;
            Habilidad3();
        }

        timeSinceLastShot += Time.deltaTime;
        //Basic Shoot
        if (Input.GetMouseButton(0) && !gunData.reloading)
        {
            shootInput?.Invoke();
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
        }else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        //ReloadInput
        if (Input.GetKeyDown(ReloadKey))
        {
            reloadInput?.Invoke();
        }

        if (gunData.currentAmmo <= 0)
        {
            reloadInput?.Invoke();
        }

        if (Input.GetKeyDown(CancelReloadKey))
        {
            cancelReloadInput?.Invoke();
        }

        //Aim
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, gunData.maxDistance, aimColliderLayerMask))
        {
            //Va directo al punto de colision
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            Debug.DrawLine(ray.origin, raycastHit.point, Color.red);
        }
        else
        {
            //Va hasta la distancia maxima y luego lo que dios quiera
            debugTransform.position = ray.GetPoint(gunData.maxDistance);
            mouseWorldPosition = ray.GetPoint(gunData.maxDistance);
            Debug.DrawLine(ray.origin, ray.GetPoint(gunData.maxDistance), Color.green);
        }

        //HabilitiesCooldown
        CooldownHab(ref currentCooldownHab1, cooldownHab1, ref isHab1OnCooldown, imageHab1, textHab1);
        CooldownHab(ref currentCooldownHab2, cooldownHab2, ref isHab2OnCooldown, imageHab2, textHab2);
        CooldownHab(ref currentCooldownHab3, cooldownHab3, ref isHab3OnCooldown, imageHab3, textHab3);
        
    }*/

    private new void OnDestroy()
    {
        if (_ply_PlayerHealth.instance != null)
        {
            _ply_PlayerHealth.instance.OnHealthChanged -= HandleHealthChanged;
        }
    }

    public override void Habilidad1()
    {
        Debug.Log("Lanzar bomba de humo");
        if (cantidadHabilidad1Actual < cantidadMaxHabilidad1)
        {
            // Spawn a new ability object in front of the player
            GameObject newHabilidad1Object = Instantiate(prefabHabilidad1, playerObj.transform.position + playerObj.transform.forward * distanciaSpawnHabilidad1, Quaternion.identity);
            newHabilidad1Object.GetComponent<NetworkObject>().Spawn(ClientID); //NetworkPrefab

            Rigidbody newHabilidad1ObjectRigidbody = newHabilidad1Object.GetComponent<Rigidbody>();

            // Apply force to the ability object
            newHabilidad1ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiroH1, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad1Objects.Add(newHabilidad1Object);

            // Increment the current number of ability objects
            cantidadHabilidad1Actual++;
            Destroy(newHabilidad1Object, 5f);

            // Increase player movement speed
            playerMovementScript.moveSpeed += 6;
            // Start a coroutine to reset the values after a delay
            StartCoroutine(ResetMovementValues());
        }
        else
        {
            // Destroy the first spawned object of this type and remove it from the list
            GameObject firstHabilidad1Object = habilidad1Objects[0];
            habilidad1Objects.RemoveAt(0);
            Destroy(firstHabilidad1Object);

            // Spawn a new ability object in front of the player
            GameObject newHabilidad1Object = Instantiate(prefabHabilidad1, playerObj.transform.position + playerObj.transform.forward * distanciaSpawnHabilidad1, Quaternion.identity);
            newHabilidad1Object.GetComponent<NetworkObject>().Spawn(ClientID); //NetworkPrefab

            Rigidbody newHabilidad1ObjectRigidbody = newHabilidad1Object.GetComponent<Rigidbody>();

            // Apply force to the ability object
            newHabilidad1ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiroH1, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad2Objects.Add(newHabilidad1Object);

            // Increase player movement speed
            playerMovementScript.moveSpeed += 6;
            // Start a coroutine to reset the values after a delay
            StartCoroutine(ResetMovementValues());
        }
    }

    public override void Habilidad2()
    {
        Debug.Log("Spawnear Baliza de curaci�n");
        if (cantidadHabilidad2Actual < cantidadMaxHabilidad2)
        {
            // Spawn a new ability object in front of the player
            GameObject newHabilidad2Object = Instantiate(prefabHabilidad2, playerObj.transform.position + playerObj.transform.forward * distanciaSpawnHabilidad2, Quaternion.identity);
            newHabilidad2Object.GetComponent<NetworkObject>().Spawn(ClientID); //NetworkPrefab

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
            newHabilidad2Object.GetComponent<NetworkObject>().Spawn(ClientID); //NetworkPrefab

            Rigidbody newHabilidad2ObjectRigidbody = newHabilidad2Object.GetComponent<Rigidbody>();

            // Apply force to the ability object
            newHabilidad2ObjectRigidbody.AddForce(playerObj.transform.forward * fuerzaDeTiroH2, ForceMode.Impulse);

            // Add the new object to the list of spawned objects
            habilidad2Objects.Add(newHabilidad2Object);
        }
    }

    public override void Habilidad3()
    {
        Debug.Log("Agrandar");

        Debug.Log("Reduce speed");
        // Reduce player movement speed
        playerMovementScript.moveSpeed -= 2;
        // Start a coroutine to reset the values after a delay
        StartCoroutine(ResetMovementValues());

        //Increase health and scale
        playerHealth.IncreaseHealth();
    }

    [ServerRpc(RequireOwnership = false)]
    public override void Shoot_ServerRpc()
    {
        Debug.Log("Disparo3");

        Vector3 aimDir = mouseWorldPosition - spawnBulletPosition.position;
        //Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        //NetworkPrefab
        GameObject bullet = Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        bullet.GetComponent<NetworkObject>().Spawn(ClientID);
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
