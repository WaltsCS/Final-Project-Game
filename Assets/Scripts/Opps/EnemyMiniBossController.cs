using UnityEngine;
using UnityEngine.AI;

public class EnemyMiniBossController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Transform player;
    private NavMeshAgent agent;

    private int hitCount = 0;
    [SerializeField] private int maxHits = 10;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("[EnemyMiniBossController] No NavMeshAgent found on parent.");
        else
            agent.speed = moveSpeed;

        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("[EnemyMiniBossController] No Player found in scene with tag 'Player'.");
    }

    private void Update()
    {
        if (player == null || agent == null) return;
        agent.SetDestination(player.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            hitCount += 1;
            if (hitCount >= maxHits)
            {
                Destroy(gameObject);
            }
        }
    }
}
