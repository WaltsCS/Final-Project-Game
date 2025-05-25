using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab to instantiate
    public float projectileSpeed = 10f; // Speed of the projectile
    public float shootInterval = 2f;   // Time between shots
    public Transform shootPoint;        // Optional: spawn point for projectiles

    private GameObject player;          // Reference to the player
    private float timeSinceLastShot = 0f; // Timer for shooting interval

    void Start()
    {
        // Find the player using the "Player" tag
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found with tag 'Player'.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Increment timer
            timeSinceLastShot += Time.deltaTime;

            // Shoot when the interval is reached
            if (timeSinceLastShot >= shootInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null)
        {
            // Use shootPoint if assigned, otherwise use enemy's position
            Vector3 spawnPosition = shootPoint != null ? shootPoint.position : transform.position;

            // Instantiate projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Calculate direction to player
            Vector3 direction = (player.transform.position - spawnPosition).normalized;

            // Rotate projectile to face the player
            projectile.transform.rotation = Quaternion.LookRotation(direction);

            // Set projectile velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
            else
            {
                Debug.LogError("Projectile prefab does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogError("Projectile prefab is not assigned.");
        }
    }
}
