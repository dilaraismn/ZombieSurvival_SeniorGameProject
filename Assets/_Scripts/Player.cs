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
    [SerializeField] private GameObject takeDamageUI;

    [SerializeField] private GameObject chestOpen, chestClosed;
        
    [SerializeField] private Camera playerCamera;

    [Header("AUDIO CLIPS")] 
    [SerializeField] private AudioClip walkSFX;
    [SerializeField] private AudioClip shootSFX, healthKitSFX, bulletKitSFX, chestSFX;
    
    public int bulletCount = 100;
    public float playerHealth = 100;
    
    public bool canShoot = true; //TODO
    public bool isLetter = false;
    private bool isFlashLightOpen = false;
    private bool isAntidoteTaken = false;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
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
            _audioSource.clip = shootSFX;
            _audioSource.loop = true;
            _audioSource.pitch = 1;
            _audioSource.volume = .4f;
            _audioSource.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _audioSource.Stop();
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

        if (Input.GetKeyDown(KeyCode.W))
        {
            _audioSource.clip = walkSFX;
            _audioSource.loop = true;
            _audioSource.pitch = .5f;
            _audioSource.volume = .2f;
            _audioSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _audioSource.Stop();
        }
    }

    private void PlayInteractSFX(AudioClip clipToPlay, float pitchValue)
    {
        _audioSource.volume = 2;
        _audioSource.pitch = pitchValue;
        _audioSource.PlayOneShot(clipToPlay);
    }
    
    private void Interact()
    {
        float interactionDistance = 8f;
            
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

                PlayInteractSFX(healthKitSFX,1);
                
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

                PlayInteractSFX(healthKitSFX,1);

                Destroy(raycastHit.transform.gameObject);
                HealthControl();
            }
            
            if (raycastHit.transform.tag == "SmallBulletBox")
            {
                PlayInteractSFX(bulletKitSFX, .8f);

                Destroy(raycastHit.transform.gameObject);
                bulletCount += 50;
                BulletCountControl();
            }
            
            if (raycastHit.transform.tag == "BigBulletBox")
            {
                PlayInteractSFX(bulletKitSFX, .8f);
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
                PlayInteractSFX(chestSFX, 1);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            TakeDamage(5);
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
        takeDamageUI.SetActive(true);
        HealthControl();
        StartCoroutine(CloseTakeDamageIndicator());
    }
    
    IEnumerator CloseTakeDamageIndicator()
    {
        yield return new WaitForSeconds(1f);
        takeDamageUI.SetActive(false);
    }

    IEnumerator CloseObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
