using UnityEngine;

public class EnemyBullet : BaseBullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        Debug.Log($"EnemyBullet collided with: {other.gameObject.name}");
        base.OnCollisionEnter(other);
    }
}