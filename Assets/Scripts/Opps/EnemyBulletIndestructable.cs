using UnityEngine;

public class EnemyBulletIndestructable : BaseBullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        Debug.Log($"EnemyBullet collided with: {other.gameObject.name}");

        if (other.gameObject.CompareTag("Scene"))
        {
            return;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            // If it hits another enemy, destroy the bullet
            Destroy(gameObject);
            return;
        }

        // If it hits the playe damage the player 
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
    }
}
