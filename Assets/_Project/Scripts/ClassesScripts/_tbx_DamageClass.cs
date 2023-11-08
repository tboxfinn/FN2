using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class _tbx_DamageClass : _tbx_BaseClass
{
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


    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    public override void Awake()
    {
        // Get the player movement script
        playerMovementScript = GetComponent<_tbx_PlayerMovementScript>();
    }

    public override void Start()
    {
        if (!IsLocalPlayer) return;

        base.Start();
        // Save the initial values of moveSpeed and jumpForce
        initialMoveSpeed = playerMovementScript.moveSpeed;
        initialJumpForce = playerMovementScript.jumpForce;

        //Set the bullets to the magazine size
        actualBullets = magazineSize;
    }

    public void Update()
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
        
    }

    public override void Habilidad1()
    {
        Debug.Log("Habilidad 1- Bomba Veneno");
        // Create a new object
        GameObject bombaVeneno = Instantiate(objectToThrow, attackPoint.position, camTransform.rotation);

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

    public override void Habilidad2()
    {
        Debug.Log("Habilidad 2- SpeedBoost");
        // Increase damage, player movement speed, and jump height
        playerMovementScript.moveSpeed += 6;
        playerMovementScript.jumpForce += 10;

        // Start a coroutine to reset the values after a delay
        StartCoroutine(ResetMovementValues());
    }

    public override void Habilidad3()
    {
        Debug.Log("Habilidad 3- Damage");
    }

    public override void Shoot()
    {
        Debug.Log("Disparo2");

        Vector3 aimDir = mouseWorldPosition - spawnBulletPosition.position;
        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }

    // Coroutine to reset the movement values after a delay
    private IEnumerator ResetMovementValues()
    {
        yield return new WaitForSeconds(effectDuration);

        // Reset the moveSpeed and jumpForce properties of the playerMovementScript
        playerMovementScript.moveSpeed = initialMoveSpeed;
        playerMovementScript.jumpForce = initialJumpForce;
    }
}