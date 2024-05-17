using System;
using System.Collections;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    [Header("Enemy Life Parameter")] 
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    [Header("Enemy Visual")] 
    [SerializeField] private GameObject enemyVisual;
    [SerializeField] private GameObject lifeBarEnemyVisual;

    [Header("Spawn Enemy")] 
    private SpawnEnemy spawnEnemy;
    
    
    // Event for enemy life
    public event EventHandler<OnDamageTakenEventArgs> OnDamageTaken;

    public class OnDamageTakenEventArgs : EventArgs
    {
        public int currentHealthEvent;
    }
    
    void Start()
    {
        spawnEnemy = FindObjectOfType<SpawnEnemy>();
        currentHealth = maxHealth;
    }

    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;
        
        OnDamageTaken?.Invoke(this, new OnDamageTakenEventArgs()
        {
            currentHealthEvent = currentHealth
        });

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EnemyDeath();
        }
    }
    
    private void EnemyDeath()
    {
        Collider2D enemyCollider = GetComponent<Collider2D>();
        enemyCollider.enabled = false;
        
        enemyVisual.SetActive(false);
        lifeBarEnemyVisual.SetActive(false);
        
        spawnEnemy.Quota--;

        if (spawnEnemy.Quota < 0)
        {
            spawnEnemy.Quota = 0;
        }

        StartCoroutine(DestroyGameobject());
    }   

    IEnumerator DestroyGameobject()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
