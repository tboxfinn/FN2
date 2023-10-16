using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _tbx_DamageClass : _tbx_BaseClass
{
    // Reference to the player movement script
    public _tbx_PlayerMovementScript playerMovementScript;

    // Duration of the effect in seconds
    public float effectDuration;

    // Initial values of moveSpeed and jumpForce
    [SerializeField] private float initialMoveSpeed;
    [SerializeField] private float initialJumpForce;

    public override void Start()
    {
        // Save the initial values of moveSpeed and jumpForce
        initialMoveSpeed = playerMovementScript.moveSpeed;
        initialJumpForce = playerMovementScript.jumpForce;
    }

    public override void Habilidad1()
    {
        Debug.Log("Habilidad 1- Damage");
    }

    public override void Habilidad2()
    {
        Debug.Log("Habilidad 2- SpeedBoost");
        // Increase damage, player movement speed, and jump height
        playerMovementScript.moveSpeed += 3;
        playerMovementScript.jumpForce += 5;

        // Start a coroutine to reset the values after a delay
        StartCoroutine(ResetMovementValues());
    }

    public override void Habilidad3()
    {
        Debug.Log("Habilidad 3- Damage");
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