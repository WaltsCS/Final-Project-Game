using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Tooltip("How long until this projectile auto-destroys (seconds)")]
    [SerializeField] private float timeToLive;

    private void Start()
    {
        // Schedule self-destruction
        Invoke("DestroyThis", timeToLive);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}