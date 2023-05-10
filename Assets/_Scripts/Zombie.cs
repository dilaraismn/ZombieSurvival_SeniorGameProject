using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int zombieHealth = 100;
    [SerializeField] private int bulletDamage = 25;

    private void Update()
    {
        if (zombieHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        print("zombie is dead");
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            print("KURŞUN YEDİM");
            zombieHealth -= bulletDamage;
            print($"zombie health: {zombieHealth}");
        }
    }
}
