using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyFinalBossHealth : MonoBehaviour
{
    ///Reference to the main boss controller
    private EnemyFinalBoss bossController;

    private void Awake()
    {
        bossController = GetComponent<EnemyFinalBoss>();
        if (bossController == null)
            Debug.LogError("[EnemyFinalBossHealth] Missing EnemyFinalBoss component!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        ///Only respond to player shots
        if (!collision.gameObject.CompareTag("PlayerBullet"))
            return;

        ///Destroy the incoming bullet
        Destroy(collision.gameObject);

        ///Apply one point of damage (or customize)
        bossController.TakeDamage(1);
    }
}

