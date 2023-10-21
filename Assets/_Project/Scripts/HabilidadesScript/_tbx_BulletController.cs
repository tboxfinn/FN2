using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _tbx_BulletController : MonoBehaviour
{
    private Rigidbody bulletRb;

    [SerializeField] private float projectileLifeTime;
    [SerializeField] private float speed = 20f;

    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Ground hit");
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Debug.Log("Wall hit");
            Destroy(gameObject);
        }
        
    }

    void Update()
    {
        Destroy(gameObject, projectileLifeTime);
    }
}
