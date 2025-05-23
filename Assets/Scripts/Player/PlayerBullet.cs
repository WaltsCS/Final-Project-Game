using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    [SerializeField] private float speed = 20f;
    // todo: change lifetime
    // todo: when bullet hits wall, destroy it
    [SerializeField] private float lifetime = 3f;

    // todo: add particle effects
    // todo: remove optional header
    [Header("Effects (Optional)")]
    [SerializeField] private GameObject hitEffectPrefab;

    //todo: delete this when bullet hit wall is implemented
    private float lifetimeTimer;

    void Awake()
    {
        lifetimeTimer = lifetime;
    }

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Countdown the lifetime
        lifetimeTimer -= Time.deltaTime;

        // If the lifetime is over, destroy the bullet
        if (lifetimeTimer <= 0f)
        {
            DestroyBullet();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // todo: when bullet hits
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
