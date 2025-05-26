using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemySeekerShooter : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed at which the enemy moves toward the player")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Shooting")]
    [Tooltip("Projectile prefab to fire (must have a Rigidbody and EnemyBullet.cs on it)")]
    [SerializeField] protected GameObject projectilePrefab;
    [Tooltip("Speed of the fired projectile")]
    [SerializeField] protected float projectileSpeed = 10f;
    [Tooltip("Time between shots")]
    [SerializeField] protected float shootInterval = 2f;
    [Tooltip("Local point to spawn projectiles (optional)")]
    [SerializeField] protected Transform shootPoint;

    [Header("Obstacle Avoidance")]
    [Tooltip("Tag of obstacles to avoid")]
    [SerializeField] protected string obstacleTag = "Obstacle";
    [Tooltip("How strongly the enemy avoids obstacles")]
    [SerializeField] protected float avoidanceStrength = 5f;
    [Tooltip("How far ahead to check for obstacles")]
    [SerializeField] protected float avoidanceRayDistance = 2f;

    protected Transform player;
    protected float shootTimer;
    private Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("[EnemySeekerShooter] No Player found in scene with tag 'Player'.");
    }

    protected virtual void Update()
    {
        if (player == null) return;

        // Face the player
        Vector3 dir = (player.position - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }

        // Shoot on interval
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot(dir);
            shootTimer = shootInterval;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (player == null) return;

        // Move toward the player
        Vector3 moveDelta = (player.position - transform.position).normalized
                            * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDelta);
    }

    protected virtual void Shoot(Vector3 direction)
    {
        // 1) Compute a horizontal-only direction
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0;
        Vector3 dir = toPlayer.normalized;

        // 2) Spawn & orient
        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position;
        var proj = Instantiate(
            projectilePrefab,
            spawnPos,
            Quaternion.LookRotation(dir, Vector3.up)
        );

        // 3) Give it velocity
        var prb = proj.GetComponent<Rigidbody>();
        if (prb != null)
            prb.linearVelocity = dir * projectileSpeed;
    }
}
