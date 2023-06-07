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

    [SerializeField] private GameObject healthFullWarning;
    [SerializeField] private GameObject antidoteTakenMessage, antidoteIcon, noAntidoteWarning;
    [SerializeField] private GameObject takeQuest, questUI, quest1, quest2, questCompletedMessage;

    [SerializeField] private GameObject chestOpen, chestClosed;
        
    [SerializeField] private Camera playerCamera;
    
    public int bulletCount = 50;
    public float playerHealth = 100;
    
    public bool canShoot = true; //TODO
    public bool isLetter = false;
    private bool isFlashLightOpen = false;
    private bool isAntidoteTaken = false;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        
        if (isLetter && Input.GetKeyDown(KeyCode.Escape))
        {
            isLetter = false;
            takeQuest.SetActive(false);
            questUI.SetActive(true);
            quest1.SetActive(true);
        }
    }

    private void Interact()
    {
        float interactionDistance = 4f;
            
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit, interactionDistance))
        {
            if (raycastHit.transform.tag == "SmallHealthKit")
            {
                if (playerHealth == 100)
                {
                    healthFullWarning.SetActive(true);
                    StartCoroutine(CloseObject(healthFullWarning));
                    return;
                }
                
                if (playerHealth > 90)
                {
                    playerHealth = 100;
                }
                else
                {
                    playerHealth += 10;
                }
                Destroy(raycastHit.transform.gameObject);
                HealthControl();
            }
            
            if (raycastHit.transform.tag == "BigHealthKit")
            {
                if (playerHealth == 100)
                {
                    healthFullWarning.SetActive(true);
                    StartCoroutine(CloseObject(healthFullWarning));
                    return;
                }
                
                if (playerHealth > 70)
                {
                    playerHealth = 100;
                }
                else
                {
                    playerHealth += 30;
                }
                Destroy(raycastHit.transform.gameObject);
                HealthControl();
            }
            
            if (raycastHit.transform.tag == "SmallBulletBox")
            {
                Destroy(raycastHit.transform.gameObject);
                bulletCount += 50;
                BulletCountControl();
            }
            
            if (raycastHit.transform.tag == "BigBulletBox")
            {
                Destroy(raycastHit.transform.gameObject);
                bulletCount += 80;
                BulletCountControl();
            }
            
            if (raycastHit.transform.tag == "Letter")
            {
                takeQuest.SetActive(true);
                isLetter = true;
            }
            
            if (raycastHit.transform.tag == "Chest")
            {
                chestClosed.SetActive(false);
                chestOpen.SetActive(true);
                antidoteTakenMessage.SetActive(true);
                antidoteIcon.SetActive(true);
                isAntidoteTaken = true;
                quest1.SetActive(false);
                quest2.SetActive(true);
                StartCoroutine(CloseObject(antidoteTakenMessage));
            }
            if (raycastHit.transform.tag == "Peter")
            {
                if (!isAntidoteTaken)
                {
                    noAntidoteWarning.SetActive(true);
                    Peter.instance.PlayNoAnim();
                    StartCoroutine(CloseObject(noAntidoteWarning));
                    return;
                }
                antidoteIcon.SetActive(false);
                quest2.SetActive(false);
                questUI.SetActive(false);
                Peter.instance.PlayThankfulAnim();
                questCompletedMessage.SetActive(true);
                StartCoroutine(CloseObject(questCompletedMessage));
            }
        }
    }
    
    private void Shoot()
    {
        if (bulletCount <= 0)
        {
            //TODO disabole muzzleflas
            canShoot = false;
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

    IEnumerator CloseObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
