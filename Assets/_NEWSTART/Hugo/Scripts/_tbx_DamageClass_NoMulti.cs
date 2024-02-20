using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class _tbx_DamageClass_NoMulti : MonoBehaviour
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
    [SerializeField] public int health;
    [SerializeField] public int maxHealth = 100;

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
    public float timeSinceLastShot;
    public int magazineSize;
    public int actualBullets;
    public float reloadTime;
    [SerializeField] public float timeSinceReloadStarted;
    [SerializeField] public bool isReloading;

    [Header("Raycast")]
    public float raycastDistance;
    [SerializeField] private Vector3 mouseWorldPosition;

    [Header("BaseReferences")]
    private Camera camPlayer;
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

    // Reference to the player movement script
    public _tbx_PlayerMovementScript_NoMulti playerMovementScript_NoMulti;

    [Header("Cosas Inicio")]
    // Duration of the effect in seconds
    public float effectDuration;
    // Initial values of moveSpeed and jumpForce
    [SerializeField] private float initialMoveSpeed;
    [SerializeField] private float initialJumpForce;

    [Header("References")]
    public Transform camTransform;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public Canvas canvas;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    public ulong ClientID { get; private set; }

    public void Awake()
    {
        // Get the player movement script
        playerMovementScript_NoMulti = GetComponent<_tbx_PlayerMovementScript_NoMulti>();
    }

    public void Start()
    {

        canvas.GetComponent<Canvas>().enabled = true;

        // Save the initial values of moveSpeed and jumpForce
        initialMoveSpeed = playerMovementScript_NoMulti.moveSpeed;
        initialJumpForce = playerMovementScript_NoMulti.jumpForce;

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

        shootInput += Shooting;
        reloadInput += StartReload;
        cancelReloadInput += CancelReload;

        camPlayer = GameObject.Find("CamaraDamage").GetComponent<Camera>();
    }

    public void Update()
    {

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

        //Basic Shoot
        if (Input.GetMouseButton(0) && !gunData.reloading)
        {   
            Vector3 shootingDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            shootInput?.Invoke(mouseWorldPosition, shootingDirection);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            
            // Update aimDir on the client side
            UpdateAimDirection(shootingDirection);
            
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
        Debug.Log("Screen Width : " + Screen.width);
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

    private void UpdateAimDirection(Vector3 shootingDirection)
    {
        aimDir = shootingDirection;
    }

    public void CooldownHab(ref float currentCooldown, float maxCooldown, ref bool isOnCooldown, Image skillImage, TMP_Text skillText)
    {

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


    
    public void Habilidad1()
    {
        Debug.Log("Habilidad 1- Bomba Veneno");
        // Create a new object
        GameObject bombaVeneno = Instantiate(objectToThrow, attackPoint.position, camTransform.rotation);

        // Get the rigidbody of the new object
        Rigidbody bombaVenRb = bombaVeneno.GetComponent<Rigidbody>();

        //Calculate the direction to throw the object in
        Vector3 forceDirection = camPlayer.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit,500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //Add force to the bombaVeneno
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        bombaVenRb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    public void Habilidad2()
    {
        Debug.Log("Habilidad 2- SpeedBoost");
        // Increase damage, player movement speed, and jump height
        playerMovementScript_NoMulti.moveSpeed += 6;
        playerMovementScript_NoMulti.jumpForce += 10;

        // Start a coroutine to reset the values after a delay
        StartCoroutine(ResetMovementValues());
    }

    public void Habilidad3()
    {
        Debug.Log("Habilidad 3- Damage");
    }

    public void Shoot()
    {
        Debug.Log("Disparo2");

        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        
    }

    // Coroutine to reset the movement values after a delay
    private IEnumerator ResetMovementValues()
    {
        yield return new WaitForSeconds(effectDuration);

        // Reset the moveSpeed and jumpForce properties of the playerMovementScript
        playerMovementScript_NoMulti.moveSpeed = initialMoveSpeed;
        playerMovementScript_NoMulti.jumpForce = initialJumpForce;
    }

    public void Shooting(Vector3 clientMouseWorldPosition, Vector3 shootingDirection)
    {
        if (gunData.currentAmmo > 0)
        {
            if (gun.CanShoot())
            {
                /*if (Physics.Raycast(gun.muzzle.position, gun.muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name);

                }*/
                aimDir = shootingDirection;

                Shoot();
                gunData.currentAmmo--;
                gun.timeSinceLastShot = 0;
                gun.OnGunShot();
            }
        }
    }

    
    public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(gun.Reload());
        }
    }

    
    public void CancelReload()
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
        health -= (int)damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthBar()
    {
        //healthBar.value = health.Value / maxHealth.Value;
        Debug.Log("Health: " + health);
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
        health = maxHealth;
        UpdateHealthBar();
    }
}
