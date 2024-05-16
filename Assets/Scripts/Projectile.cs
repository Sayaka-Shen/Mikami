using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Stats")] 
    private Rigidbody2D projectileRigidbody;
    
    [Header("Collision")] 
    private BoxCollider2D projectileCollider;

    private void Start()
    {
        projectileRigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out HeroMovement heroComponent))
        {
            Destroy(gameObject);
        }
    }
}
