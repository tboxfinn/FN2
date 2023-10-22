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

        //Save the initial value of moveSpeed
        initialMoveSpeed = playerMovementScript.moveSpeed;
        //Set the bullets to the magazine size
        actualBullets = magazineSize;
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

    public override void Shoot()
    {
        Debug.Log("Disparo3");

        Vector3 aimDir = mouseWorldPosition - spawnBulletPosition.position;
        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
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
