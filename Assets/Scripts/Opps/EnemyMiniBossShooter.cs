using UnityEngine;

public class EnemyMiniBossShooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float sideShotAngle = 30f;
    [SerializeField] private Transform centerShootPoint;
    [SerializeField] private Transform leftShootPoint;
    [SerializeField] private Transform rightShootPoint;

    private Transform player;
    private float shootTimer;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("[EnemyMiniBossShooter] No Player found in scene with tag 'Player'.");
    }

    private void Update()
    {
        if (player == null) return;

        // Rotate toward player
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0;
        if (toPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toPlayer, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }

        // Shooting logic
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot(toPlayer.normalized);
            shootTimer = shootInterval;
        }
    }

    private void Shoot(Vector3 direction)
    {
        ShootSingle(direction, centerShootPoint != null ? centerShootPoint.position : transform.position);

        Vector3 toPlayer = direction;
        toPlayer.y = 0;
        toPlayer.Normalize();

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
}
