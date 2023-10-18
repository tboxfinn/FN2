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

    [Header("DisparoBase")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce;
    public float bulletDamage;
    public float fireRate;
    public int magazineSize;
    public int actualBullets;

    [Header("Raycast")]
    public float raycastDistance;

    [Header("BaseReferences")]
    public Camera cam;
    
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

        //Disparo
        actualBullets = magazineSize;
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
        }

        //HabilitiesCooldown
        CooldownHab(ref currentCooldownHab1, cooldownHab1, ref isHab1OnCooldown, imageHab1, textHab1);
        CooldownHab(ref currentCooldownHab2, cooldownHab2, ref isHab2OnCooldown, imageHab2, textHab2);
        CooldownHab(ref currentCooldownHab3, cooldownHab3, ref isHab3OnCooldown, imageHab3, textHab3);

        // Raycast from player to center of camera
        Vector3 cameraCenter = cam.transform.position;
        Vector3 direction = (cameraCenter - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastDistance))
        {
            // Draw debug ray
            Debug.DrawRay(transform.position, direction * hit.distance, Color.red);

            // Aim gun at hit point
            Vector3 aimPoint = hit.point;
            firePoint.LookAt(aimPoint);
        }
        else
        {
            // Draw debug ray
            Debug.DrawRay(transform.position, direction * raycastDistance, Color.green);

            // Aim gun at maximum range
            Vector3 aimPoint = transform.position + direction * raycastDistance;
            firePoint.LookAt(aimPoint);
        }
        
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
