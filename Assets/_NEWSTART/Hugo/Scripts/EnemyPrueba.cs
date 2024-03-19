using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrueba : MonoBehaviour
{
    public Material scannedMaterial; // Material to use when the enemy is scanned

    private Renderer renderer; // Renderer component of the enemy
    private Material originalMaterial; // Original material of the enemy

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
    }

    void OnEnable()
    {
        Radar.OnEnemyDetected += ReactToDetection;
    }

    void OnDisable()
    {
        Radar.OnEnemyDetected -= ReactToDetection;
    }

    

    void ReactToDetection(GameObject enemy)
    {
        if (enemy == gameObject)
        {
            Debug.LogWarning("Enemy detected: " + enemy.name);

            // Change the material of the enemy
            renderer.material = scannedMaterial;

            // Change the material back to the original material after 1 second
            Invoke("ResetMaterial", 1f);
        }
    }

    void ResetMaterial()
    {
        renderer.material = originalMaterial;
    }
}
