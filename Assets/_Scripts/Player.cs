using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float shootRate = 0.2f;
    [SerializeField] private float shootForce = 2000;
    [SerializeField] private GameObject bulletPref, muzzleVFX;
    [SerializeField] private Transform bulletPoint;

    public bool canShoot = true; //TODO
    
    void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(muzzleVFX, bulletPoint.transform.position, Quaternion.identity);
        GameObject bullet = Instantiate(bulletPref, bulletPoint.transform.position, bulletPref.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
        //TODO: AudioSource.PlayClipAtPoint(shootSound, transform.position); 

        canShoot = false;
        Invoke("EnableShooting", shootRate);
    }
    
    private void EnableShooting()
    {
        canShoot = true;
    }
}
