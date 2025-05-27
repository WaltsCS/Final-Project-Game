using UnityEngine;

public class PlayerBullet : BaseBullet
{
    [Header("Bullet Properties")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;

    // todo: add particle effects
    // todo: remove optional header
    [Header("Effects (Optional)")]
    [SerializeField] private GameObject hitEffectPrefab;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        // todo: when bullet hits
        if (other.gameObject.CompareTag("Miniboss"))
        {

            var enemy = other.gameObject.GetComponent<EnemyMiniBossController>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
        base.OnCollisionEnter(other);
    }
}