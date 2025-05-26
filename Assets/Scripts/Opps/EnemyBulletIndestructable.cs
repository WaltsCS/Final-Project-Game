using UnityEngine;

public class EnemyBulletIndestructable : BaseBullet
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"EnemyBullet collided with: {other.gameObject.name}");

        if (other.gameObject.CompareTag("Scene"))
        {
            return;
        }

        // If it hits the playe damage the player 
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
    }
}
