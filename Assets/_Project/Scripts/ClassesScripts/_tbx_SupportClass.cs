using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _tbx_SupportClass : _tbx_BaseClass
{
    private _ply_PlayerHealth playerHealth;

    private void Start()
    {
        // ... (Código de búsqueda del objeto _ply_PlayerHealth)

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += HandleHealthChanged;
        }
        else
        {
            Debug.LogError("El objeto encontrado no tiene el componente _ply_PlayerHealth");
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
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

        if (playerHealth != null)
        {
            playerHealth.IncreaseHealth();
        }
    }

    private void HandleHealthChanged(int newHealth, Vector3 newScale)
    {
        Debug.Log("Vida actual: " + newHealth);
        Debug.Log("Nueva escala: " + newScale);
    }
}
