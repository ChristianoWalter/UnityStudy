using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    //public static HealthController instance;

    [Header("Life Parameters")]
    [SerializeField] protected bool isDead;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isInvencible;
    //[SerializeField] protected HealthBarController healthBar;

    protected virtual void Awake()
    {
        //instance = this;
        currentHealth = maxHealth;
        //if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
    }

    protected virtual void Death()
    {
        Debug.Log(currentHealth);
    }

    public void TakeDamage(float _damage)
    {
        if (isInvencible) return;
        if (currentHealth > 0)
        {
            currentHealth = Mathf.Max(currentHealth - _damage, 0);
            DamageEffect();
            Debug.Log(currentHealth);
            if (currentHealth == 0)
            {
                Death();
            }
        }
        else if (currentHealth == 0)
        {
            Death();
        }
    }

    protected virtual void DamageEffect()
    {
        //if (healthBar != null) healthBar.SetHealth(currentHealth);
    }

    public void TakeHeal(float heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
        }
        //if (healthBar != null) healthBar.SetHealth(currentHealth);

        Debug.Log(currentHealth);
    }
}
