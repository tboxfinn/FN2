using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class _ply_PlayerHealth : NetworkBehaviour
{
    public int health;
    private int originalHealth;

    // Evento para notificar cambios en la vida y escala
    public event Action<int, Vector3> OnHealthChanged;

    // Variables para el aumento de vida temporal
    private bool isHealthIncreased = false;
    private float healthIncreaseDuration = 5f; // Duración en segundos

    void Start()
    {
        originalHealth = health;
    }

    void Update()
    {
        if (isHealthIncreased)
        {
            // Actualizar la duración del aumento de vida
            healthIncreaseDuration -= Time.deltaTime;

            // Verificar si el tiempo ha expirado
            if (healthIncreaseDuration <= 0f)
            {
                // Restaurar la vida original
                health = originalHealth;
                isHealthIncreased = false;

                // Notificar a los suscriptores del evento con la vida actual y la escala original
                OnHealthChanged?.Invoke(health, Vector3.one);
            }
        }

        if (health == 0)
        {
            Debug.Log("Te quedaste sin vida");
        }
    }

    public void TakeDamage()
    {
        health--;
        // Notificar a los suscriptores del evento con la vida actual y la escala actual
        OnHealthChanged?.Invoke(health, transform.localScale);
    }

    public void IncreaseHealth()
    {
        // Solo aumentar la vida si no está aumentada actualmente
        if (!isHealthIncreased)
        {
            // Guardar la vida actual como original
            originalHealth = health;

            // Aumentar la vida
            health++;
            isHealthIncreased = true;

            // Configurar la duración del aumento de vida
            healthIncreaseDuration = 5f;

            // Calcular la nueva escala (por ejemplo, duplicar la escala actual)
            Vector3 newScale = transform.localScale * 2f;

            // Aplicar la nueva escala al objeto
            transform.localScale = newScale;

            // Notificar a los suscriptores del evento con la vida actual y la nueva escala
            OnHealthChanged?.Invoke(health, newScale);
        }
        else
        {
            Debug.Log("La vida ya está aumentada");
        }
    }
}
