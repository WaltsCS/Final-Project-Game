using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemySeekerShooter : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed at which the enemy moves toward the player")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Shooting")]
    [Tooltip("Projectile prefab to fire (must have a Rigidbody and EnemyBullet.cs on it)")]
    [SerializeField] private GameObject projectilePrefab;
    [Tooltip("Speed of the fired projectile")]
    [SerializeField] private float projectileSpeed = 10f;
    [Tooltip("Time between shots")]
    [SerializeField] private float shootInterval = 2f;
    [Tooltip("Local point to spawn projectiles (optional)")]
    [SerializeField] private Transform shootPoint;

    [Header("Obstacle Avoidance")]
    [Tooltip("Tags of obstacles to slow down near (e.g. \"Wall\", \"Rock\", etc.)")]
    [SerializeField] private string[] obstacleTags;
    [Tooltip("Factor (0–1) to multiply speed when an obstacle is close")]
    [SerializeField] private float slowFactor = 0.3f;
    [Tooltip("How far ahead (meters) to check for obstacles")]
    [SerializeField] private float checkDistance = 0.5f;
    [Tooltip("Radius of the sphere used for obstacle detection")]
    [SerializeField] private float sphereRadius = 0.4f;

    private Transform player;
    private float shootTimer;
    private Rigidbody rb;

    // Record the Y position at spawn time so we can lock to it
    private float fixedY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("[EnemySeekerShooter] No Player found in scene with tag 'Player'.");

        // Record starting Y to keep them from popping up
        fixedY = transform.position.y;
    }

    private void Update()
    {
        if (player == null) return;

        // Face the player (purely horizontal)
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0;
        if (toPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toPlayer, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }

        // Shooting on interval
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot(toPlayer.normalized);
            shootTimer = shootInterval;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // 1) Compute horizontal move direction
        Vector3 rawDir = player.position - transform.position;
        rawDir.y = 0;
        Vector3 moveDir = rawDir.normalized;

        // 2) Determine speed, slowing if any obstacle with one of the tags is within checkDistance
        float speed = moveSpeed;
        if (moveDir.sqrMagnitude > 0.01f && obstacleTags != null && obstacleTags.Length > 0)
        {
            // SphereCast forward to detect obstacles
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                sphereRadius,
                moveDir,
                checkDistance
            );

            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    // Check each of the obstacleTags
                    for (int i = 0; i < obstacleTags.Length; i++)
                    {
                        if (hit.collider.CompareTag(obstacleTags[i]))
                        {
                            speed = moveSpeed * slowFactor;
                            goto SkipFurtherChecks;
                        }
                    }
                }
            }
        }

        SkipFurtherChecks:

        // 3) Compute new horizontal position, then clamp Y to fixedY
        Vector3 desiredlinearVelocity = moveDir * speed;
        Vector3 newPos = rb.position + new Vector3(desiredlinearVelocity.x, 0f, desiredlinearVelocity.z) * Time.fixedDeltaTime;
        newPos.y = fixedY;

        // 4) Move the rigidbody
        rb.MovePosition(newPos);
    }

    private void Shoot(Vector3 direction)
    {
        if (projectilePrefab == null) return;

        // Flatten direction to horizontal
        direction.y = 0;
        direction.Normalize();

        Vector3 spawnPos = (shootPoint != null) ? shootPoint.position : transform.position;
        GameObject proj = Instantiate(
            projectilePrefab,
            spawnPos,
            Quaternion.LookRotation(direction, Vector3.up)
        );

        Rigidbody prb = proj.GetComponent<Rigidbody>();
        if (prb != null)
            prb.linearVelocity = direction * projectileSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If hit by a player bullet, destroy bullet, play VFX, then destroy this enemy
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);

            if (ParticleFX.Instance != null)
                ParticleFX.Instance.PlayEnemyDeathVFX(transform.position);

            Destroy(gameObject);
            return;
        }

        // If pushed up by wall collision, force Y back down
        Vector3 vel = rb.linearVelocity;
        vel.y = 0;
        rb.linearVelocity = vel;

        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }

    private void OnCollisionStay(Collision collision)
    {
        // Maintain Y‐clamp while sliding against walls
        Vector3 vel = rb.linearVelocity;
        vel.y = 0;
        rb.linearVelocity = vel;

        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }
}
