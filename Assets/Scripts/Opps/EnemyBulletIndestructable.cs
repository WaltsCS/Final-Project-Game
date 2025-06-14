using UnityEngine;

public class EnemyBulletIndestructable : BaseBullet
{

    protected override void OnCollisionEnter(Collision other)
    {
        // Bullet can be destroyed other than player bullets
        if (other.gameObject.CompareTag("PlayerBullet") || other.gameObject.CompareTag("EnemyBullet"))
        {
            return;
        }
        base.OnCollisionEnter(other);
    }
}
