using UnityEngine;
using UnityEngine.AI;

public class EnemyFinalBossController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    private NavMeshAgent agent;
    private Transform player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("[EnemyMiniBossController] No NavMeshAgent found on parent.");
        else
            agent.speed = moveSpeed;

        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("[EnemyFinalBoss] No Player found in scene with tag 'Player'.");
    }

    private void Update()
    {
        if (player == null || agent == null) return;
        agent.SetDestination(player.position);
    }
}
