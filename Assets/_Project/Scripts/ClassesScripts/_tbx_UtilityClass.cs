using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class _tbx_UtilityClass : NetworkBehaviour
{
    public Vector3 aimDir;

    [Header("Keybinds")]
    public KeyCode Hab1;
    public KeyCode Hab2;
    public KeyCode Hab3;
    public KeyCode ReloadKey;
    public KeyCode CancelReloadKey;
    public KeyCode ActionKey;

    [Header("Stats")]
    [SerializeField] private NetworkVariable<float> health = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private NetworkVariable<float> maxHealth = new NetworkVariable<float>(150f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Habilidad1")]
    //public Sprite spriteHab1;
    public Image imageHab1Normal;
    public Image imageHab1;
    public TMP_Text textHab1;
    public float cooldownHab1;
    [SerializeField] public bool isHab1OnCooldown = false;
    [SerializeField] public float currentCooldownHab1;

    [Header("Habilidad2")]
    //public Sprite spriteHab2;
    public Image imageHab2Normal;
    public Image imageHab2;
    public TMP_Text textHab2;
    public float cooldownHab2;
    [SerializeField] public bool isHab2OnCooldown = false;
    [SerializeField] public float currentCooldownHab2;

    [Header("Habilidad3")]
    //public Sprite spriteHab3;
    public Image imageHab3Normal;
    public Image imageHab3;
    public TMP_Text textHab3;
    public float cooldownHab3;
    [SerializeField] public bool isHab3OnCooldown = false;
    [SerializeField] public float currentCooldownHab3;

    [Header("BasicShoot")]
    public float fireRate;
    //public float timeSinceLastShot;
    public int magazineSize;
    public int actualBullets;
    public float reloadTime;
    [SerializeField] public float timeSinceReloadStarted;
    [SerializeField] public bool isReloading;

    [Header("Raycast")]
    public float raycastDistance;
    public Vector3 mouseWorldPosition;

    [Header("BaseReferences")]
    public Camera camPlayer;
    public int teamID;
    [SerializeField] public LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] public Transform debugTransform;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject pfBulletProjectile;
    [SerializeField] public Transform spawnBulletPosition;
    [SerializeField] public GunData gunData;
    [SerializeField] public Gun gun;

    [Header("InputCosos")]
    //public static Action shootInput;
    public static Action reloadInput;
    public static Action cancelReloadInput;
    public delegate void ShootInputDelegate(Vector3 mouseWorldPosition, Vector3 shootingDirection);
    public event ShootInputDelegate shootInput;

    [Header("Utility Class")]
    public float fuerzaDeTiro;
    public float distanciaSpawnHabilidad1;

    [Header("Habilidad1")]
    public GameObject PrefabBalaStun;
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
    public Canvas canvas;

    public ulong ClientID { get; private set; }

    public void Start()
    {
        if (!IsOwner) return;

        canvas.GetComponent<Canvas>().enabled = true;

        gunData.fireRate = 300;
        // Find the player object in the scene
        //player = GameObject.FindGameObjectWithTag("Player");
        velocidadDeTiroInicial = gunData.fireRate;
        
        //Stats
        health.Value = maxHealth.Value;
        
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

        shootInput += ShootingServerRpc;
        reloadInput += StartReloadServerRpc;
        cancelReloadInput += CancelReloadServerRpc;

        camPlayer = GameObject.Find("CamaraUtility").GetComponent<Camera>();
    }

    public void Update()
    {
        if (!IsOwner) return;

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
            Habilidad3ServerRpc();
        }
  
        //Basic Shoot
        if (Input.GetMouseButton(0) && !gunData.reloading)
        {
            Vector3 shootingDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            shootInput?.Invoke(mouseWorldPosition, shootingDirection);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            
            // Update aimDir on the client side
            UpdateAimDirectionClientRpc(shootingDirection);
            
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
        Ray ray = camPlayer.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, gunData.maxDistance, aimColliderLayerMask))
        {
            //Va directo al punto de colision
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            Debug.DrawLine(ray.origin, raycastHit.point, Color.red);
        }
        else if (Physics.Raycast(ray, out RaycastHit raycastHit1, gunData.maxDistance))
        {
            //Va directo al punto de colision
            debugTransform.position = raycastHit1.point;
            mouseWorldPosition = raycastHit1.point;
            Debug.DrawLine(ray.origin, raycastHit1.point, Color.yellow);
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

        //aimDir = mouseWorldPosition - spawnBulletPosition.position;
    }

    [ClientRpc]
    private void UpdateAimDirectionClientRpc(Vector3 shootingDirection)
    {
        aimDir = shootingDirection;
    }

    public void CooldownHab(ref float currentCooldown, float maxCooldown, ref bool isOnCooldown, Image skillImage, TMP_Text skillText)
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if (isOnCooldown)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                currentCooldown = 0;
                isOnCooldown = false;
                if (skillImage != null)
                {
                    skillImage.fillAmount = 0;
                }
                if (skillText != null)
                {
                    skillText.text = "";
                }
            }
            else
            {
                if (skillImage != null)
                {
                    skillImage.fillAmount = currentCooldown / maxCooldown;
                }
                if (skillText != null)
                {
                    skillText.text = Mathf.Ceil(currentCooldown).ToString();
                }
            }
        }
    }

    // Spawn a new ability object
    public void Habilidad1()
    {
        Debug.Log("Utility - StunBullet");
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
    public void Habilidad2()
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
    [ServerRpc]
    public void Habilidad3ServerRpc()
    {
        Debug.Log("Habilidad 3 - Barrera");
        StartCoroutine(EscudoGiratorio());
    }

    private IEnumerator EscudoGiratorio()
    {
        GameObject Escudo = Instantiate(prefabHabilidad3, playerObj.transform.position, Quaternion.identity);
        Escudo.GetComponent<NetworkObject>().SpawnWithOwnership(ClientID);
        //Escudo.transform.parent = playerObj.transform;
        Escudo.transform.position = playerObj.transform.position;

        yield return new WaitForSeconds(tiempoHabilidad3);
        Destroy(Escudo);
    }

    public void Shoot()
    {
        if (Hab1Selected && gunData.currentAmmo == 1)
        {
            Debug.Log("Disparo2");

            //Instantiate(PrefabBalaStun, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            //NetworkPrefab
            GameObject bullet = Instantiate(PrefabBalaStun, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            bullet.GetComponent<NetworkObject>().SpawnWithOwnership(ClientID);
        }
        else
        {
            Debug.Log("Disparo1");

            //Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            //NetworkPrefab
            GameObject bullet = Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            bullet.GetComponent<NetworkObject>().SpawnWithOwnership(ClientID);
        }

        
    }

    [ServerRpc]
    public void ShootingServerRpc(Vector3 clientMouseWorldPosition, Vector3 shootingDirection)
    {
        if (gunData.currentAmmo > 0)
        {
            if (gun.CanShoot())
            {

                aimDir = shootingDirection;

                Shoot();
                gunData.currentAmmo--;
                gun.timeSinceLastShot = 0;
                gun.OnGunShot();
            }
        }
    }

    [ServerRpc]
    public void StartReloadServerRpc()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(gun.Reload());
        }
    }

    [ServerRpc]
    public void CancelReloadServerRpc()
    {
        if (gunData.reloading)
        {
            StopCoroutine(gun.Reload());
            gunData.reloading = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit");
            Destroy(other.gameObject);
            TakeDamage(gunData.damage);
        }
    }

    public void TakeDamage(float damage)
    {
        health.Value -= damage;
        UpdateHealthBar();
        if (health.Value <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthBar()
    {
        //healthBar.value = health.Value / maxHealth.Value;
        Debug.Log("Health: " + health.Value);
    }

    public void Die()
    {
        // Find all spawn points
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (spawnPoints.Length == 0)
    {
        Debug.LogError("No spawn points found");
        return;
    }

        // Select a random spawn point
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform;

        // Teleport the player to the spawn point
        transform.position = spawnPoint.position;

        // Reset the player's health
        health.Value = maxHealth.Value;
        UpdateHealthBar();
    }
}