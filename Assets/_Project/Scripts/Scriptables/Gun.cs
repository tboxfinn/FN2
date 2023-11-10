using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;
    [SerializeField] public Transform muzzle;
    [SerializeField] public _tbx_BaseClass tbxBase;

    public float timeSinceLastShot;

    private void Start()
    {
        //_tbx_BaseClass.shootInput += Shoot;
        //_tbx_BaseClass.reloadInput += StartReload;
        //cancelReloadInput += CancelReload;

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    /*public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }*/

    public IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;

    }

    public void CancelReload()
    {
        if (gunData.reloading)
        {
            StopCoroutine(Reload());
            gunData.reloading = false;
        }
    }

    public bool CanShoot() => !gunData.reloading && timeSinceLastShot >= 1 / (gunData.fireRate / 60);

    /*public void Shoot()
    {
        if (gunData.currentAmmo >0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name);
                    
                }
                
                tbxBase.Shoot_ServerRpc();
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }*/

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public void OnGunShot()
    {
        Debug.Log("GunShot");
    }
}
