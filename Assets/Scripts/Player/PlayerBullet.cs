using UnityEngine;

public class PlayerBullet : BaseBullet
{
    [SerializeField] private float damage = 10f;

    // todo: add particle effects
    // todo: remove optional header
    [Header("Effects (Optional)")]
    [SerializeField] private GameObject hitEffectPrefab;

    protected override void OnCollisionEnter(Collision other)
    {
        Debug.Log($"PlayerBullet collided with: {other.gameObject.name}");
        base.OnCollisionEnter(other);
    }

}