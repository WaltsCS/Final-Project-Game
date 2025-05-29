using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MiniBossHealth : MonoBehaviour
{
    // Cache once at Start
    private EnemyMiniBossController controller;

    void Start()
    {
        // Walk up to find the controller on the parent
        controller = GetComponentInParent<EnemyMiniBossController>();
        if (controller == null)
            Debug.LogError("[MiniBossHealth] Couldn't find EnemyMiniBossController in parents!");
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only react to the player's bullets
        if (!collision.gameObject.CompareTag("PlayerBullet"))
            return;

        // Destroy the bullet
        Destroy(collision.gameObject);

        // Tell the boss to take damage
        controller.TakeDamage();
    }
}
