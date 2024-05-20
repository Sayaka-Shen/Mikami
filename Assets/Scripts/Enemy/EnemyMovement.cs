using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Movement")] 
    [SerializeField] private float enemySpeed = 1.5f;
    private Rigidbody2D rigidbodyEnemy;
    private Animator enemyAnimator;
    private float direction = -1f;

    [Header("Enemy Statistics")] 
    [SerializeField] private float enemyJumpForce = 10f;

    [Header("Hero")] 
    [SerializeField] private HeroMovement heroMovement;

    [Header("Enemy Detection Player")] 
    [SerializeField] private Transform enemyDetectionPoint;
    [SerializeField] private float circleEnemyDetectionCastRange;
    [SerializeField] private LayerMask playerLayer;
    private EnemyState currentState = EnemyState.Patrol;
    
    [Header("Enemy Detection Allies")]
    [SerializeField] private LayerMask enemyLayer;
    private Vector2 directionVector;

    [Header("Enemy Attack")] 
    private EnemyAttack enemyAttack;

    [Header("Spawn Ennemy")] 
    private SpawnEnemy spawnEnemy;
    
    private enum EnemyState
    {
        Patrol,
        AttackMeleeFast,
        AttackMeleeSlow,
        AttackRange
    }
    
    private void Start()
    {
        rigidbodyEnemy = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponent<EnemyAttack>();
        heroMovement = FindObjectOfType<HeroMovement>();
        spawnEnemy = FindObjectOfType<SpawnEnemy>();
        enemyAnimator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        CheckForPlayer();
        CheckEnemyState();
        CheckForAllies();
        enemyAnimator.SetFloat("Speed", Mathf.Abs(rigidbodyEnemy.velocity.x));
    }

    private void Patrol()
    {
        if (heroMovement.transform.position.x < transform.position.x)
        {
            direction = -1f;
        }
        else
        {
            direction = 1f;
        }
        
        if ((direction == 1 && transform.localScale.x > 0f) || (direction == -1 && transform.localScale.x < 0f))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
        
        rigidbodyEnemy.velocity = new Vector2(direction * enemySpeed, rigidbodyEnemy.velocity.y);
    }
    
    private void CheckForPlayer()
    {
        RaycastHit2D hitObject = Physics2D.CircleCast(
            enemyDetectionPoint.position, 
            circleEnemyDetectionCastRange, 
            Vector2.right,
            circleEnemyDetectionCastRange, 
            playerLayer);
        
        if (hitObject.transform != null)
        {
            if (hitObject.transform.TryGetComponent(out HeroAttack heroComponent))
            { 
                if (circleEnemyDetectionCastRange >= 10)
                {
                    currentState = EnemyState.AttackRange;
                }
                else if(circleEnemyDetectionCastRange == 1f)
                {
                    currentState = EnemyState.AttackMeleeSlow;
                }
                else if(circleEnemyDetectionCastRange == 0.8f)
                {
                    currentState = EnemyState.AttackMeleeFast;
                }
            }
            else
            {
                currentState = EnemyState.Patrol;
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    private void CheckEnemyState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.AttackMeleeFast:
                enemyAttack.AttackFastMelee();
                break;
            case EnemyState.AttackMeleeSlow:
                enemyAttack.AttackSlowMelee();
                break;
            case EnemyState.AttackRange:
                enemyAttack.AttackRange();
                break;
            default:
                break;
        }
    }

    private void CheckForAllies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f, enemyLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != gameObject && currentState == EnemyState.Patrol && this.gameObject.CompareTag("EnemyMelee"))
            {
                EnemyLife enemyComponent = collider.GetComponent<EnemyLife>();
                
                if (enemyComponent != null)
                {
                    rigidbodyEnemy.velocity = new Vector2(rigidbodyEnemy.velocity.x, enemyJumpForce);
                    break;
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        // Draw detection zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyDetectionPoint.position, circleEnemyDetectionCastRange);
        Gizmos.DrawRay(transform.position, directionVector);
    }
}
