using UnityEngine;

public class EnemyBullet : BaseBullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        Debug.Log($"EnemyBullet collided with: {other.gameObject.name}");

        // Check for scene collision
        if (other.gameObject.CompareTag("Scene") || other.gameObject.CompareTag("Miniboss"))
        {
            return;
        }

        base.OnCollisionEnter(other);
    }
}