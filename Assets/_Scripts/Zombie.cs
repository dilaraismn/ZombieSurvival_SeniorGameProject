using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private Transform playerObj;
    [SerializeField] private int zombieHealth = 100;
    [SerializeField] private int bulletDamage = 25;

    private Animator animator;
    private NavAgentExample navAgentScript;
    private NavMeshAgent navAgent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgentScript = GetComponent<NavAgentExample>();
    }
    
    private void TakeDamage()
    {
        navAgentScript.enabled = false;
        navAgent.SetDestination(playerObj.position);

        zombieHealth -= bulletDamage;
        
        if (zombieHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.Play("TakeHit");
        }
    }
    
    private void Die()
    {
        animator.Play("Die");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }
}
