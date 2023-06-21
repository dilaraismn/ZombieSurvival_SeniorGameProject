using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private Transform playerObj;
    [SerializeField] private GameObject bloodVFX;
    [SerializeField] private int zombieHealth = 100;
    [SerializeField] private int bulletDamage = 25;
    [SerializeField] private AudioClip growl, bite, getHit, dead;

    private Animator animator;
    private NavAgentExample navAgentScript;
    private NavMeshAgent navAgent;
    private CapsuleCollider collider;
    private AudioSource audioSource;
    private GameObject player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgentScript = GetComponent<NavAgentExample>();
        collider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.clip = growl;
        audioSource.Play();
    }

    private void Update()
    {
        float maxDistance = 20;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        float volume = Mathf.Clamp01(1f - (distance / maxDistance));
        audioSource.volume = volume;
    }
    
    public void ChangeSX(AudioClip clipToPlay)
    {
        audioSource.loop = false;
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }

    private void TakeDamage()
    {
        zombieHealth -= bulletDamage;
        navAgentScript.enabled = false;
        ChangeSX(getHit);

        if (zombieHealth <= 0)
        {
            Die();
            navAgent.isStopped = true;
        }
        else
        {
            animator.Play("TakeHit");
            navAgent.ResetPath();
            navAgent.SetDestination(playerObj.position);
        }
    }
    
    private void Die()
    {
        animator.Play("Die");
        ChangeSX(dead);
        navAgentScript.enabled = false;
        navAgent.enabled = false;
        collider.enabled = false;
        gameObject.isStatic = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Instantiate(bloodVFX, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            TakeDamage();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            ChangeSX(bite);
        }
    }
}
