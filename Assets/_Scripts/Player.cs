using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    
    [SerializeField] private float shootRate = 0.2f;
    [SerializeField] private float shootForce;
    [SerializeField] private GameObject flashLight;
    
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private TMP_Text bulletCountText;

    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Camera playerCamera;
    
    public int bulletCount = 50;
    public float playerHealth = 100;
    
    public bool canShoot = true; //TODO
    private bool isFlashLightOpen = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        BulletCountControl();
        HealthControl();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashLightOpen = !isFlashLightOpen;
            flashLight.SetActive(isFlashLightOpen);
        }
    }

    private void Shoot()
    {
        if (bulletCount == 0)
        {
            //TODO disabole muzzleflas
            return;
        }
       
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = (hit.point - transform.position);
            //Vector3 direction = hit.point.normalized;

            GameObject bullet = Instantiate(bulletPref, bulletPoint.transform.position, Quaternion.LookRotation(direction));
            bullet.GetComponent<Rigidbody>().AddForce(direction * shootForce, ForceMode.Impulse);
           
            bulletCount -= 1;
            BulletCountControl();
        }
        
        canShoot = false;
        Invoke("EnableShooting", shootRate);
    }
    
    private void EnableShooting()
    {
        canShoot = true;
    }

    private void BulletCountControl()
    {
        bulletCountText.text = bulletCount.ToString();
    }

    private void HealthControl()
    {
        healthBar.fillAmount = playerHealth / 100;
        healthText.text = playerHealth.ToString();
    }

    public void TakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
        HealthControl();
    }
}
