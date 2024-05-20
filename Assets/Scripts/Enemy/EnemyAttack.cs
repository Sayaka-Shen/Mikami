using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Enemy Distance Parameter")] 
    [SerializeField] private Transform baseShootPoint;
    [SerializeField] private GameObject projectilePrefab;
    private float projectileSpeed = 12f;
    [SerializeField] private float projectileIntervalTimer = 1f;
    private float projectileTime;

    [Header("Enemy Melee Attack Parameter")] 
    [SerializeField] private LayerMask heroLayer;
    [SerializeField] private Transform enemyMeleeDetectionPointAttack;
    [SerializeField] private float meleeAttackCooldown = 1.5f;
    [SerializeField] private float circleMeleeEnemyAttackCastRange = 0.5f;
    private bool isEnemyMeleeAttacking = false;

    [Header("Enemy Damage")] 
    private int FastEnemyAttackDamage = 5;
    private int SlowEnemyAttackDamage = 10;

    [Header("Hero Entity")] 
    [SerializeField] private GameObject heroTransform;
    private HeroLife heroLife;
    
    [Header("Animator")] 
    private Animator enemyAnimator;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        heroLife = FindObjectOfType<HeroLife>();
    }

    #region Functions Attack Range

    public void AttackRange()
    {
        projectileTime -= Time.deltaTime;

        if (projectileTime > 0) return;

        projectileTime = projectileIntervalTimer;

        enemyAnimator.SetTrigger("IsDistanceShooting");

        GameObject instanceProjectilePrefab = Instantiate(projectilePrefab, baseShootPoint.position, Quaternion.identity);
        Rigidbody2D rigidbodyProjectile = instanceProjectilePrefab.GetComponent<Rigidbody2D>();

        Vector2 direction = (heroTransform.transform.position - baseShootPoint.position).normalized;
        rigidbodyProjectile.velocity = direction * projectileSpeed;
    }

    #endregion
    
    #region Functions Attack Melee
    
    public void AttackFastMelee()
    {
        if (isEnemyMeleeAttacking) return;
        
        // Detect ennemies around
        RaycastHit2D hitObject = Physics2D.CircleCast(
            enemyMeleeDetectionPointAttack.position, 
            circleMeleeEnemyAttackCastRange, 
            Vector2.right, 
            circleMeleeEnemyAttackCastRange, 
            heroLayer);
        

        if (hitObject.transform != null)
        {
            if (hitObject.transform.TryGetComponent(out HeroLife heroComponent))
            {
                StartCoroutine(PerformFastMeleeAttack());    
            }
        }   
    }

    public void AttackSlowMelee()
    {
        if (isEnemyMeleeAttacking) return;
        
        // Detect ennemies around
        RaycastHit2D hitObject = Physics2D.CircleCast(
            enemyMeleeDetectionPointAttack.position, 
            circleMeleeEnemyAttackCastRange, 
            Vector2.right, 
            circleMeleeEnemyAttackCastRange, 
            heroLayer);
        

        if (hitObject.transform != null)
        {
            if (hitObject.transform.TryGetComponent(out HeroLife heroComponent))
            {
                StartCoroutine(PerformSlowMeleeAttack());    
            }
        } 
    }
    
    private IEnumerator PerformFastMeleeAttack()
    {
        isEnemyMeleeAttacking = true;
        enemyAnimator.SetTrigger("IsFastEnemyAttacking");
        heroLife.HeroTakeDamage(FastEnemyAttackDamage);

        yield return new WaitForSeconds(meleeAttackCooldown);
        isEnemyMeleeAttacking = false;
    }

    private IEnumerator PerformSlowMeleeAttack()
    {
        isEnemyMeleeAttacking = true;
        enemyAnimator.SetTrigger("IsFastEnemyAttacking");
        enemyAnimator.SetTrigger("IsSlowEnemyAttacking");
        heroLife.HeroTakeDamage(SlowEnemyAttackDamage);

        yield return new WaitForSeconds(meleeAttackCooldown);
        isEnemyMeleeAttacking = false;
    }
    
    #endregion
}
