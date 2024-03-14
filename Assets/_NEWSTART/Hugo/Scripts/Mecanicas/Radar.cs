using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public static event Action<GameObject> OnEnemyDetected; // Evento que se dispara cuando se detecta un jugador

    [Header("Scan Settings")]
    public float scanRadius = 5f; // Define el radio del escaneo
    public float scanInterval = 1f; // Define el intervalo de escaneo
    public LayerMask scanLayers; // Define la capa de los objetos a escanear
    
    [Header("Life Settings")]
    public float lifeTime = 10f; // Define el tiempo de vida del objeto

    private bool isStuck = false; // Indica si el objeto se ha pegado a una superficie
    private float scanTimer = 0f; // Temporizador para el escaneo

    // Start is called before the first frame update
    void Start()
    {
        // Destruye el objeto después de un cierto tiempo
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStuck)
        {
            // Incrementa el temporizador y realiza un escaneo si ha pasado el intervalo de escaneo
            scanTimer += Time.deltaTime;
            if (scanTimer >= scanInterval)
            {
                Scan();
                scanTimer = 0f;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Cuando el objeto colisiona con una superficie, se "pega" a ella
        GetComponent<Rigidbody>().isKinematic = true;
        isStuck = true;
    }

    void Scan()
    {
        // Realiza un escaneo en un radio alrededor del objeto, solo detectando objetos en la capa de los jugadores
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scanRadius, scanLayers);
        foreach (var hitCollider in hitColliders)
        {
            // Comprueba si el objeto detectado tiene la etiqueta "Enemy"
            if (hitCollider.CompareTag("Enemy"))
            {
                // Aquí puedes hacer lo que necesites con los enemigos detectados
                Debug.LogWarning("Enemy detected: " + hitCollider.name);
                // Trigger the OnEnemyDetected event
                OnEnemyDetected?.Invoke(hitCollider.gameObject);
            }
        }
    }
}
