using UnityEngine;

public class EnemyBullet : BaseBullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        Debug.Log($"EnemyBullet collided with: {other.gameObject.name}");
        
        // Check for player bullet collision
        if (other.gameObject.CompareTag("Player Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        // Check for scene collision
        if (other.gameObject.CompareTag("Scene"))
        {
            return;
        }

        base.OnCollisionEnter(other);
    }
}