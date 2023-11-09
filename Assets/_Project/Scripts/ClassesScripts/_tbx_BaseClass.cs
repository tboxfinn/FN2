using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
using Unity.Netcode;
using System;

public class _tbx_BaseClass : NetworkBehaviour
{
    [Header("Keybinds")]
    public KeyCode Hab1;
    public KeyCode Hab2;
    public KeyCode Hab3;
    public KeyCode ReloadKey;
    public KeyCode CancelReloadKey;
    public KeyCode ActionKey;

    [Header("Stats")]
    public float health;
    public float maxHealth;

    [Header("Habilidad1")]
    //public Sprite spriteHab1;
    public Image imageHab1Normal;
    public Image imageHab1;
    public TMP_Text textHab1;
    public float cooldownHab1;
    [SerializeField] public bool isHab1OnCooldown=false;
    [SerializeField] public float currentCooldownHab1;

    [Header("Habilidad2")]
    //public Sprite spriteHab2;
    public Image imageHab2Normal;
    public Image imageHab2;
    public TMP_Text textHab2;
    public float cooldownHab2;
    [SerializeField] public bool isHab2OnCooldown=false;
    [SerializeField] public float currentCooldownHab2;

    [Header("Habilidad3")]
    //public Sprite spriteHab3;
    public Image imageHab3Normal;
    public Image imageHab3;
    public TMP_Text textHab3;
    public float cooldownHab3;
    [SerializeField] public bool isHab3OnCooldown=false;
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

    public static Action shootInput;
    public static Action reloadInput;
    public static Action cancelReloadInput;

    public virtual void Awake()
    {
        //Get Animator
        //animator = GetComponentInChildren<Animator>();
    }
    public virtual void Start()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        //Get Main Cam
        //virtualCamera = GetComponent<CinemachineVirtualCamera>();

        //Stats
        /*health = maxHealth;
        
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
        */

    }

    [ServerRpc (RequireOwnership = false)]
    public void TakeDamage_ServerRpc(float damage)
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        health -= damage;
        if (health <= 0)
        {
           health = 0;
        }
    }

    [ServerRpc (RequireOwnership = false)]
    public void Heal_ServerRpc(float heal)
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        health += heal;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
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

    public virtual void MakeAction()
    {
        Debug.Log("Accion");
    }

    public virtual void Habilidad1()
    {
        Debug.Log("Habilidad 1");
    }

    public virtual void Habilidad2()
    {
        Debug.Log("Habilidad 2");
    }

    public virtual void Habilidad3()
    {
        Debug.Log("Habilidad 3");
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void Shoot_ServerRpc()
    {
        Debug.Log("Disparo");
        
    }

}
