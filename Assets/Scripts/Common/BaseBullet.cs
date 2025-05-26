using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] protected float timeToLive = 10f;

    protected virtual void Start()
    {
        Invoke(nameof(DestroyThis), timeToLive);
    }


    protected virtual void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

    protected void DestroyThis()
    {
        Destroy(gameObject);
    }

}
