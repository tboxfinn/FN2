using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class _tbx_BaseClass : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode Hab1;
    public KeyCode Hab2;
    public KeyCode Hab3;
    public KeyCode ActionKey;

    [Header("Stats")]
    public float health;
    public float maxHealth;

    [Header("Habilidad1")]
    public Sprite spriteHab1;
    public Image imageHab1Normal;
    public Image imageHab1;
    public TMP_Text textHab1;
    public float cooldownHab1;
    [SerializeField] private bool isHab1OnCooldown=false;
    [SerializeField] private float currentCooldownHab1;

    [Header("Habilidad2")]
    public Sprite spriteHab2;
    public Image imageHab2Normal;
    public Image imageHab2;
    public TMP_Text textHab2;
    public float cooldownHab2;
    [SerializeField] private bool isHab2OnCooldown=false;
    [SerializeField] private float currentCooldownHab2;

    [Header("Habilidad3")]
    public Sprite spriteHab3;
    public Image imageHab3Normal;
    public Image imageHab3;
    public TMP_Text textHab3;
    public float cooldownHab3;
    [SerializeField] private bool isHab3OnCooldown=false;
    [SerializeField] private float currentCooldownHab3;

    public float fireRate;
    public int magazineSize;
    public int actualBullets;

    [Header("Raycast")]
    public float raycastDistance;
    public Vector3 mouseWorldPosition;

    [Header("BaseReferences")]
    public Camera cam;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Animator animator;
     [SerializeField] public Transform pfBulletProjectile;
    [SerializeField] public Transform spawnBulletPosition;

    public virtual void Awake()
    {
        //Get Main Cam
        cam = Camera.main;
        //Get Animator
        animator = GetComponent<Animator>();
    }
    public virtual void Start()
    {
        //Get Main Cam
        //virtualCamera = GetComponent<CinemachineVirtualCamera>();

        //Stats
        health = maxHealth;
        Hab1 = KeyCode.Alpha1;
        Hab2 = KeyCode.Alpha2;
        Hab3 = KeyCode.Alpha3;
        Debug.Log("Base Class");

        //Habilidades
        imageHab1.fillAmount = 0;
        imageHab2.fillAmount = 0;
        imageHab3.fillAmount = 0;

        textHab1.text = "";
        textHab2.text = "";
        textHab3.text = "";
        
        if(spriteHab1 != null && imageHab1 != null)
        {
            imageHab1.sprite = spriteHab1;
            imageHab1Normal.sprite = spriteHab1;
        }
        if(spriteHab2 != null && imageHab2 != null)
        {
            imageHab2.sprite = spriteHab2;
            imageHab2Normal.sprite = spriteHab2;
        }
        if(spriteHab3 != null && imageHab3 != null)
        {
            imageHab3.sprite = spriteHab3;
            imageHab3Normal.sprite = spriteHab3;
        }

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
           health = 0;
        }
    }

    public void Heal(float heal)
    {
        health += heal;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Update()
    {
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

        //Basic Shoot
        if (Input.GetMouseButtonDown(0) && actualBullets > 0)
        {
            Shoot();
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            actualBullets--;
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        //Aim
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            Debug.DrawLine(ray.origin, raycastHit.point, Color.red);
        }

        //HabilitiesCooldown
        CooldownHab(ref currentCooldownHab1, cooldownHab1, ref isHab1OnCooldown, imageHab1, textHab1);
        CooldownHab(ref currentCooldownHab2, cooldownHab2, ref isHab2OnCooldown, imageHab2, textHab2);
        CooldownHab(ref currentCooldownHab3, cooldownHab3, ref isHab3OnCooldown, imageHab3, textHab3);
        
    }

    private void CooldownHab(ref float currentCooldown, float maxCooldown, ref bool isOnCooldown, Image skillImage, TMP_Text skillText)
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

    public virtual void Shoot()
    {
        Debug.Log("Disparo");
        
    }

}
