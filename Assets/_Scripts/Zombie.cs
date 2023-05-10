using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int zombieHealth = 100;
    [SerializeField] private int bulletDamage = 25;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void TakeDamage()
    {
        print("KURŞUN YEDİM");
        zombieHealth -= bulletDamage;
        
        if (zombieHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.Play("TakeDamage");
        }
    }
    
    private void Die()
    {
        print("zombie is dead");
        Destroy(this.gameObject);
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
