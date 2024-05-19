using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLife : MonoBehaviour
{
    [Header("Hero Statistics")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public int MaxHealth
    {
        get { return maxHealth; }
    }
    
    // Event when Hero take damage
    public event EventHandler<OnHeroLifeChangesEventAgrs> OnHeroLifeChanges;

    public class OnHeroLifeChangesEventAgrs : EventArgs
    {
        public int currentHealthHeroEvent;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void HeroTakeDamage(int damage)
    {
        currentHealth -= damage;
        
        OnHeroLifeChanges?.Invoke(this, new OnHeroLifeChangesEventAgrs()
        {
            currentHealthHeroEvent = currentHealth
        });

        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    public void HeroRegenLife(int life)
    {
        currentHealth += life;
        
        OnHeroLifeChanges?.Invoke(this, new OnHeroLifeChangesEventAgrs()
        {
            currentHealthHeroEvent = currentHealth
        });
    }

    private void PlayerDeath()
    {
        Destroy(gameObject);
    }
}
