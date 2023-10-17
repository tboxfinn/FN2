using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _tbx_SupportClass : _tbx_BaseClass
{
    public _tbx_PlayerMovementScript playerMovementScript;

    public float effectDuration;

    [SerializeField] private float initialMoveSpeed;

    public override void Start()
    {
        if (_ply_PlayerHealth.instance != null)
        {
            _ply_PlayerHealth.instance.OnHealthChanged += HandleHealthChanged;
        }
        else
        {
            Debug.LogError("El objeto encontrado no tiene el componente _ply_PlayerHealth");
        }
        initialMoveSpeed = playerMovementScript.moveSpeed;
    }

    private void OnDestroy()
    {
        if (_ply_PlayerHealth.instance != null)
        {
            _ply_PlayerHealth.instance.OnHealthChanged -= HandleHealthChanged;
        }
    }

    public override void Habilidad1()
    {
        Debug.Log("Habilidad 1- Support");
    }

    public override void Habilidad2()
    {
        Debug.Log("Habilidad 2- Support");
    }

    public override void Habilidad3()
    {
        Debug.Log("Habilidad 3- Support");

        Debug.Log("Reduce speed");
        // Reduce player movement speed
        playerMovementScript.moveSpeed = playerMovementScript.moveSpeed - 3;
        // Start a coroutine to reset the values after a delay
        StartCoroutine(ResetMovementValues());

        if (_ply_PlayerHealth.instance != null)
        {
            _ply_PlayerHealth.instance.IncreaseHealth();
        }

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
