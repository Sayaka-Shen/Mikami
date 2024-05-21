using System;
using System.Collections;
using UnityEngine;

public class HeroLife : MonoBehaviour
{
    [Header("Hero Statistics")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private Transform heroTransform;
    private HeroAttack heroAttack;
    private BugBar bugBar;
    private PlayerLifeBar heroLifeBar;
    private Animator animator;
    
    public int MaxHealth
    {
        get { return maxHealth; }
    }
    
    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    [Header("Death Respawn Parameter")] 
    [SerializeField] private GameObject heroVisual;
    private Vector3 lastPosition;
    //private bool isDead = false;
    
    // Event when Hero take damage
    public event EventHandler<OnHeroLifeChangesEventAgrs> OnHeroLifeChanges;

    public class OnHeroLifeChangesEventAgrs : EventArgs
    {
        public int currentHealthHeroEvent;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        heroTransform = GetComponent<Transform>();
        heroAttack = GetComponent<HeroAttack>();
        bugBar = FindObjectOfType<BugBar>();
        heroLifeBar = GetComponentInChildren<PlayerLifeBar>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (!heroAttack.AttackMode)
        {
            RaycastHit2D hit = Physics2D.Raycast(heroTransform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                lastPosition = heroTransform.position;   
            }
        }
    }

    public void HeroTakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("IsDamaged");
        
        OnHeroLifeChanges?.Invoke(this, new OnHeroLifeChangesEventAgrs()
        {
            currentHealthHeroEvent = currentHealth
        });

        if (currentHealth <= 0)
        {
            if (heroAttack.AttackMode)
            {
                DieAttackMode();
            }
        }
    }

    public void HeroRegenLife(int life)
    {
        if (heroAttack.AttackMode)
        {
            currentHealth += life;
        
            OnHeroLifeChanges?.Invoke(this, new OnHeroLifeChangesEventAgrs()
            {
                currentHealthHeroEvent = currentHealth
            });   
        }
    }

    public void ChangeLifeBarEventOnOtherScript(int currentHealth)
    {
        OnHeroLifeChanges?.Invoke(this, new OnHeroLifeChangesEventAgrs()
        {
            currentHealthHeroEvent = currentHealth
        });  
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("DeathWall"))
        {
            currentHealth = 0;
            
            OnHeroLifeChanges?.Invoke(this, new OnHeroLifeChangesEventAgrs()
            {
                currentHealthHeroEvent = currentHealth
            });

            heroVisual.SetActive(false);
            bugBar.gameObject.SetActive(false);
            heroLifeBar.gameObject.SetActive(false);
            StartCoroutine(DieNormalMode());
        }
    }

    private IEnumerator DieNormalMode()
    {
        animator.SetTrigger("IsDead");
        yield return new WaitForSeconds(0.5f);
        
        heroTransform.position = lastPosition;
        heroVisual.SetActive(true);
        bugBar.gameObject.SetActive(true);
        heroLifeBar.gameObject.SetActive(true);

        currentHealth = maxHealth;
        
        OnHeroLifeChanges?.Invoke(this, new OnHeroLifeChangesEventAgrs()
        {
            currentHealthHeroEvent = currentHealth
        });
    }

    private void DieAttackMode()
    {
        animator.SetTrigger("IsDead");

        heroTransform.position = bugBar.TeleporteArene.position;

        currentHealth = maxHealth;
        
        OnHeroLifeChanges?.Invoke(this, new OnHeroLifeChangesEventAgrs()
        {
            currentHealthHeroEvent = currentHealth
        });
    }
}
