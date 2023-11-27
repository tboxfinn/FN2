using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _tbx_ProjectileAddon : MonoBehaviour
{
    public int damage;
    private Rigidbody rb;

    private bool targetHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
            return;
        else
            targetHit = true;

        // Check if the object that was hit is the player
        rb.isKinematic = true;

    }
}
