using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] private float timeToLive = 5f;

    protected virtual void Start()
    {
        Invoke(nameof(DestroyThis), timeToLive);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
