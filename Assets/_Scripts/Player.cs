using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float shootRate = 0.2f;
    [SerializeField] private float shootForce;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private Camera playerCamera;
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
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = (hit.point - transform.position).normalized;

            GameObject bullet = Instantiate(bulletPref, bulletPoint.transform.position, Quaternion.LookRotation(direction));
            bullet.GetComponent<Rigidbody>().AddForce(direction * shootForce, ForceMode.Impulse);
        }
        
        //GameObject bullet = Instantiate(bulletPref, bulletPoint.transform.position, bulletPref.transform.rotation);
        //bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
        //TODO: AudioSource.PlayClipAtPoint(shootSound, transform.position); 

        canShoot = false;
        Invoke("EnableShooting", shootRate);
    }
    
    private void EnableShooting()
    {
        canShoot = true;
    }
}
