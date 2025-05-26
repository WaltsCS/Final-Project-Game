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
        Debug.Log($"Bullet collided with: {other.gameObject.name}");
        // todo: when bullet hits
        base.OnCollisionEnter(other);
    }


}