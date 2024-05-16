using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Enemy Projectile")] 
    [SerializeField] private Transform baseShootPoint;
    [SerializeField] private GameObject projectilePrefab;
    private GameObject instanceProjectilePrefab;
    
    [SerializeField] private int projectileSpeed = 12;
    [SerializeField] private float projectileIntervalTimer = 3f;
    private float projctileTime;

    [Header("Enemy Entity")]
    private EnemyMovement enemyMovement;

    [Header("Hero Entity")] 
    [SerializeField] private GameObject heroTransform;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void AttackRange()
    {
        projctileTime -= Time.deltaTime;

        if (projctileTime > 0) return;

        projctileTime = projectileIntervalTimer;

        instanceProjectilePrefab = Instantiate(projectilePrefab, baseShootPoint.position, this.transform.rotation);
            
        Rigidbody2D rigibodyProjectile = instanceProjectilePrefab.GetComponent<Rigidbody2D>();

        rigibodyProjectile.velocity = new Vector2((heroTransform.transform.position.x - baseShootPoint.position.x) * projectileSpeed, (heroTransform.transform.position.y - baseShootPoint.position.y) * projectileSpeed);
    }
    
    public void AttackMelee()
    {
        
    }
}
