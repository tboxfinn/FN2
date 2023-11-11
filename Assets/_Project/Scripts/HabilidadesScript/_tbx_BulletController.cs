using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class _tbx_BulletController : NetworkBehaviour
{
    [SerializeField] private float projectileLifeTime;
    [SerializeField] private float speed = 20f;

    public int teamID;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            teamID = GetComponentInParent<_tbx_BaseClass>().teamID;
        }

        GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            var player = other.GetComponent<_tbx_BaseClass>();
            // Check if the player's teamID is different from the bullet's teamID
            if (player != null && player.teamID != this.teamID)
            {
                Debug.Log("Enemy hit");
                Destroy(gameObject);
            }
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
