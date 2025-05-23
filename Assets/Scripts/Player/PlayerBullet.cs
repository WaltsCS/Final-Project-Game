using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 3f; // Time in seconds before the bullet is destroyed

    [Header("Effects (Optional)")]
    [SerializeField] private GameObject hitEffectPrefab; // Particle effect or decal to spawn on impact

    // Internal timer
    private float lifetimeTimer;

    void Awake()
    {
        // Initialize the timer
        lifetimeTimer = lifetime;
    }

    void Update()
    {
        // Move the bullet forward
        // transform.forward is the blue axis of the bullet GameObject
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Countdown the lifetime
        lifetimeTimer -= Time.deltaTime;

        // If the lifetime is over, destroy the bullet
        if (lifetimeTimer <= 0f)
        {
            DestroyBullet();
        }
    }

    // This function is called when the bullet collides with another Rigidbody or Collider
    void OnCollisionEnter(Collision collision)
    {
        // Optional: Check what the bullet hit
        // For example, if it hits an object tagged "Enemy"
        /*
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            // EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            // if (enemyHealth != null)
            // {
            //     enemyHealth.TakeDamage(damageAmount);
            // }
        }
        */

        // Spawn a hit effect if one is assigned
        if (hitEffectPrefab != null)
        {
            // Instantiate the hit effect at the point of collision
            // collision.contacts[0].point is the first point of contact
            // Quaternion.LookRotation(collision.contacts[0].normal) orients the effect with the surface normal
            Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

        // Destroy the bullet upon impact
        DestroyBullet();
    }

    // Helper function to destroy the bullet
    private void DestroyBullet()
    {
        // Destroy this bullet GameObject
        Destroy(gameObject);
    }

    // Optional: If you want to set properties from the PlayerController when spawning
    public void SetBulletProperties(float bulletSpeed, float bulletLifetime)
    {
        speed = bulletSpeed;
        lifetime = bulletLifetime;
        lifetimeTimer = lifetime; // Reset timer if lifetime is set externally
    }

}
