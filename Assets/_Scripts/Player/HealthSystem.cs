using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDamaged;
    public event Action OnDied;
    public event Action OnHealed;

    [SerializeField] private int maxHealth;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int amount)
    {
        if (currentHealth <= amount)
        {
            currentHealth = 0;
            Die();
            return;
        }

        currentHealth -= amount;
        OnDamaged?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);
        OnHealed?.Invoke();
    }

    public void Die()
    {
        OnDied?.Invoke();
    }

    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => currentHealth;
}
