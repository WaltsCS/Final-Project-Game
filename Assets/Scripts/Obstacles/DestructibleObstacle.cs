using UnityEngine;

public class DestructibleObstacle : MonoBehaviour
{
    [Tooltip("Particle effect when destroyed (optional)")]
    public GameObject destructionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // Only destroy if hit by a player bullet
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            if (destructionEffect != null)
                Instantiate(destructionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
