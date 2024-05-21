using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Stats")] 
    private Rigidbody2D projectileRigidbody;
    private int projectileDamage = 8;
    private float secondBeforeDestruction = 4f;
    
    [Header("Collision")] 
    private BoxCollider2D projectileCollider;

    [Header("Hero Entity")] 
    private HeroLife heroLife;

    private void Start()
    {
        projectileRigidbody = GetComponent<Rigidbody2D>();
        heroLife = FindObjectOfType<HeroLife>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out HeroMovement heroComponent))
        {
            heroLife.HeroTakeDamage(projectileDamage);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DestroyGameObjectAfterSeveralSeconds());
        }
    }

    private IEnumerator DestroyGameObjectAfterSeveralSeconds()
    {
        yield return new WaitForSeconds(secondBeforeDestruction);
        Destroy(gameObject);
    }
}
