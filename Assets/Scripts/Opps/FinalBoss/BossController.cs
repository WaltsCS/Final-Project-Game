using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHealth = 69;
    int currentHealth;
    public float phaseTransition1 = 70, phaseTransition2 = 40;
    enum Phase { Phase1, Phase2, Phase3, Dead }
    Phase currentPhase;
    // references to node Transforms
    [SerializeField] private Transform[] fireNodes;
    void Start()
    {
        currentHealth = maxHealth;
        currentPhase = Phase.Phase1;
        StartCoroutine(PhaseRoutine());
    }
    void Update()
    {
        // optional: flash color on damage, check destruction
    }
    IEnumerator PhaseRoutine()
    {
        while (currentPhase != Phase.Dead)
        {
            switch (currentPhase)
            {
                case Phase.Phase1: yield return PhaseBehavior(1f, 1.5f); break;
                case Phase.Phase2: yield return PhaseBehavior(0.8f, 1f); break;
                case Phase.Phase3: yield return PhaseBehavior(0.5f, 0.7f); break;
            }
            yield return null;
        }
        // play death animation
    }
    IEnumerator PhaseBehavior(float fireRate, float patternDuration)
    {
        float timer = 0;
        while (timer < patternDuration && currentPhase != Phase.Dead)
        {
            SpawnPattern(currentPhase);
            yield return new WaitForSeconds(fireRate);
            timer += fireRate;
        }
        yield break;
    }
    void SpawnPattern(Phase phase)
    {
        switch (phase)
        {
            case Phase.Phase1:
                // fire straight from two opposite nodes
                FireStraight(fireNodes[0]);
                FireStraight(fireNodes[1]);
                break;
            case Phase.Phase2:
                // spiral: rotate around and fire
                for (int i = 0; i < fireNodes.Length; i++)
                    FireAtAngle(fireNodes[i], Time.time * 100f + i * 45f);
                break;
            case Phase.Phase3:
                // mixed: occasional homing mines
                if (Random.value < 0.3f) SpawnMine();
                // plus spiral
                SpawnPattern(Phase.Phase2);
                break;
        }
    }
    // Helper methods:
    void FireStraight(Transform node)
    {
        var bullet = ObjectPooler.Instance.GetFromPool("Bullet", node.position, node.rotation);
        bullet.transform.position = node.position;
        bullet.transform.rotation = node.rotation;
        var dir = node.up; // Assuming the node's up direction is the firing direction
        bullet.GetComponent<Bullet>().Init(dir);
    }
    void FireAtAngle(Transform node, float angleDeg)
    {
        var bullet = ObjectPooler.Instance.GetFromPool("Bullet", node.position, node.rotation);
        bullet.transform.position = node.position;
        bullet.transform.rotation = node.rotation * Quaternion.Euler(0, angleDeg, 0);
        var dir = bullet.transform.forward; // Assuming the bullet's forward direction is the firing direction
        bullet.GetComponent<Bullet>().Init(dir);
    }
    void SpawnMine()
    {
        // var m = MinePool.Instance.Get();
        // m.transform.position = transform.position + Random.insideUnitSphere * 1.5f;
        // m.Init(playerTransform);
    }
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) currentPhase = Phase.Dead;
        else if (currentHealth <= phaseTransition2) currentPhase = Phase.Phase3;
        else if (currentHealth <= phaseTransition1) currentPhase = Phase.Phase2;
        // flash effect or shake
    }
}
