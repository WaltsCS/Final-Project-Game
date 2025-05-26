using UnityEngine;

public class EnemyBullet : BaseBullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        Debug.Log($"EnemyBullet collided with: {other.gameObject.name}");
        // add exception for the scene mesh collider
        if (other.gameObject.CompareTag("Scene"))
        {
            return;
        }
        base.OnCollisionEnter(other);
    }
}