using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class _tbx_DamageClass : NetworkBehaviour
{
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
    public float timeSinceLastShot;
    public int magazineSize;
    public int actualBullets;
    public float reloadTime;
    [SerializeField] public float timeSinceReloadStarted;
    [SerializeField] public bool isReloading;

    [Header("Raycast")]
    public float raycastDistance;
    public Vector3 mouseWorldPosition;

    [Header("BaseReferences")]
    public Camera cam;
    public int teamID;
    [SerializeField] public LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] public Transform debugTransform;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject pfBulletProjectile;
    [SerializeField] public Transform spawnBulletPosition;
    [SerializeField] public GunData gunData;
    [SerializeField] public Gun gun;

    public static Action shootInput;
    public static Action reloadInput;
    public static Action cancelReloadInput;

    // Reference to the player movement script
    public _tbx_PlayerMovementScript playerMovementScript;

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
        playerMovementScript = GetComponent<_tbx_PlayerMovementScript>();
    }

    public void Start()
    {
        if (!IsOwner) return;

        canvas.GetComponent<Canvas>().enabled = true;

        // Save the initial values of moveSpeed and jumpForce
        initialMoveSpeed = playerMovementScript.moveSpeed;
        initialJumpForce = playerMovementScript.jumpForce;

        //Set the bullets to the magazine size
        actualBullets = magazineSize;

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
    }

    public void Update()
    {
        if (!IsOwner) return;

        //ActionInput
        if (Input.GetKeyDown(ActionKey))
        {
            TestClientRpc();
            TestClientRpc();
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


    
    public void Habilidad1()
    {
        Debug.Log("Habilidad 1- Bomba Veneno");
        // Create a new object
        GameObject bombaVeneno = Instantiate(objectToThrow, attackPoint.position, camTransform.rotation);
        bombaVeneno.GetComponent<NetworkObject>().SpawnWithOwnership(ClientID);

        // Get the rigidbody of the new object
        Rigidbody bombaVenRb = bombaVeneno.GetComponent<Rigidbody>();

        //Calculate the direction to throw the object in
        Vector3 forceDirection = cam.transform.forward;

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
        playerMovementScript.moveSpeed += 6;
        playerMovementScript.jumpForce += 10;

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

        Vector3 aimDir = mouseWorldPosition - spawnBulletPosition.position;

        //Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        //NetworkPrefab
        GameObject bullet = Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        bullet.GetComponent<NetworkObject>().SpawnWithOwnership(ClientID);
    }

    // Coroutine to reset the movement values after a delay
    private IEnumerator ResetMovementValues()
    {
        yield return new WaitForSeconds(effectDuration);

        // Reset the moveSpeed and jumpForce properties of the playerMovementScript
        playerMovementScript.moveSpeed = initialMoveSpeed;
        playerMovementScript.jumpForce = initialJumpForce;
    }

    [ServerRpc]
    public void ShootingServerRpc()
    {
        if (gunData.currentAmmo > 0)
        {
            if (gun.CanShoot())
            {
                if (Physics.Raycast(gun.muzzle.position, gun.muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name);

                }

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

    [ServerRpc]
    private void TestServerRpc()
    {
        Debug.Log("TestServerRpc");
    }

    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("TestClientRpc");
    }
}