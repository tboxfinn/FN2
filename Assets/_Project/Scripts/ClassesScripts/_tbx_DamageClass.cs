using System.Collections;
using System.Collections.Generic;
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

    public override void Start()
    {
        // Save the initial values of moveSpeed and jumpForce
        initialMoveSpeed = playerMovementScript.moveSpeed;
        initialJumpForce = playerMovementScript.jumpForce;
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
        Debug.Log("Disparo");

        // Calculate direction of shot
        Vector3 cameraCenter = cam.transform.position;
        Vector3 direction = (cameraCenter - firePoint.position).normalized;

        // Instantiate bullet object
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Apply force to bullet object in direction of shot
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction * bulletForce, ForceMode.Impulse);
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