using UnityEngine;

public class EnemyMiniBossShooter : EnemySeekerShooter
{
    [Header("Mini Boss Shooting")]
    [Tooltip("Angle in degrees for the side shots relative to the center direction")]
    [SerializeField] private float sideShotAngle = 30f;

    [SerializeField] private Transform centerShootPoint;
    [SerializeField] private Transform leftShootPoint;
    [SerializeField] private Transform rightShootPoint;

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
} 