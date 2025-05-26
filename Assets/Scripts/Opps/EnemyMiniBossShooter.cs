using UnityEngine;
using UnityEngine.AI;

public class EnemyMiniBossShooter : EnemySeekerShooter
{
    [Header("Mini Boss Shooting")]
    [Tooltip("Angle in degrees for the side shots relative to the center direction")]
    [SerializeField] private float sideShotAngle = 30f;

    [SerializeField] private Transform centerShootPoint;
    [SerializeField] private Transform leftShootPoint;
    [SerializeField] private Transform rightShootPoint;

    private NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void FixedUpdate(){}

    protected override void Shoot(Vector3 direction)
    {
        // Center shot
        ShootSingle(direction, centerShootPoint != null ? centerShootPoint.position : transform.position);

        // Calculate side directions
        Vector3 toPlayer = direction; toPlayer.y = 0; toPlayer.Normalize();
        Vector3 leftDir = Quaternion.Euler(0, -sideShotAngle, 0) * toPlayer;
        Vector3 rightDir = Quaternion.Euler(0, sideShotAngle, 0) * toPlayer;

        ShootSingle(leftDir, leftShootPoint != null ? leftShootPoint.position : transform.position);
        ShootSingle(rightDir, rightShootPoint != null ? rightShootPoint.position : transform.position);
    }

    private void ShootSingle(Vector3 dir, Vector3 spawnPos)
    {
        var proj = Instantiate(
            projectilePrefab,
            spawnPos,
            Quaternion.LookRotation(dir, Vector3.up)
        );
        var prb = proj.GetComponent<Rigidbody>();
        if (prb != null)
            prb.linearVelocity = dir * projectileSpeed;
    }

    protected override void Update()
    {
        if (player == null) return;

        // Set the agent's destination to the player's position
        agent.SetDestination(player.position);

        // Face the player when shooting
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0;
        if (toPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toPlayer, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }

        // Shoot on interval
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot(toPlayer.normalized);
            shootTimer = shootInterval;
        }
    }
} 