using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFinalBoss : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField] private float shootInterval = 3f;

    [Header("Fire Points Root")]
    [SerializeField] private Transform shootPointsRoot;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 30f;

    [Header("Health & Phases")]
    public int maxHealth = 100;
    [Tooltip("HP percentage to switch to Phase2")] [Range(0,100)]
    public float phaseTransition1 = 50f;

    private int currentHealth;
    private enum Phase { Phase1, Phase2, Dead }
    private Phase currentPhase;

    private float shootTimer;
    private List<Transform> shootPoints;

    private void Awake()
    {
        // Gather all child shoot points
        if (shootPointsRoot != null)
        {
            shootPoints = new List<Transform>(shootPointsRoot.childCount);
            foreach (Transform child in shootPointsRoot)
                shootPoints.Add(child);
        }
        else
        {
            Debug.LogError("[EnemyFinalBoss] ShootPointsRoot not assigned.");
        }

        currentHealth = maxHealth;
        currentPhase = Phase.Phase1;
        shootTimer = shootInterval;

        StartCoroutine(PhaseRoutine());
    }

    private void Update()
    {
        // Rotate the shoot points around the boss
        if (shootPointsRoot != null)
            shootPointsRoot.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);

        // Shooting timer
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f && currentPhase != Phase.Dead)
        {
            FireAll();
            shootTimer = shootInterval;
        }
    }

    /// <summary>
    /// Call this to apply damage to the boss.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (currentPhase == Phase.Dead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        // Check phase transitions
        float hpPercent = currentHealth / (float)maxHealth * 100f;
        if (hpPercent <= 0f)
        {
            currentPhase = Phase.Dead;
            StopAllCoroutines();
            StartCoroutine(Die());
        }
        else if (hpPercent <= phaseTransition1 && currentPhase == Phase.Phase1)
        {
            currentPhase = Phase.Phase2;
            // (Optional) change parameters for Phase2
            shootInterval *= 0.8f;
            rotationSpeed *= 1.2f;
        }

        // (Optional) Visual feedback here (flash, shake, etc.)
    }

    IEnumerator PhaseRoutine()
    {
        // Loop until death
        while (currentPhase != Phase.Dead)
        {
            switch (currentPhase)
            {
                case Phase.Phase1:
                    yield return PhaseBehavior(Phase.Phase1, 1f, 2f);
                    break;
                case Phase.Phase2:
                    yield return PhaseBehavior(Phase.Phase2, 0.8f, 2.5f);
                    break;
            }
            yield return null;
        }
    }

    IEnumerator PhaseBehavior(Phase phase, float fireRate, float duration)
    {
        float timer = 0f;
        while (timer < duration && currentPhase == phase)
        {
            SpawnPattern(phase);
            yield return new WaitForSeconds(fireRate);
            timer += fireRate;
        }
    }

    void SpawnPattern(Phase phase)
    {
        switch (phase)
        {
            case Phase.Phase1:
                // Simple: fire in all directions equally
                FireAll();
                break;
            case Phase.Phase2:
                // Fire mines sometimes
                if (Random.value < 0.3f)
                    rotationSpeed += 30;
                break;
        }
    }

    private void FireAll()
    {
        foreach (var sp in shootPoints)
        {
            ///Calculate direction as outward from boss center to shoot point
            Vector3 dir = (sp.position - transform.position).normalized;
            dir.y = 0; ///keep horizontal plane
            if (dir.sqrMagnitude < 0.001f)
                dir = sp.forward; ///fallback

            ///Instantiate and orient projectile
            var proj = Instantiate(projectilePrefab, sp.position, Quaternion.LookRotation(dir, Vector3.up));
            var prb = proj.GetComponent<Rigidbody>();
            if (prb != null)
                prb.linearVelocity = dir * projectileSpeed;
        }
    }

    // void SpawnMine()
    // {
    //     var mine = ObjectPooler.Instance.GetFromPool("EnemyBullet", transform.position, Quaternion.identity);
    //     var player = GameObject.FindWithTag("Player");
    //     if (mine != null && player != null)
    //         mine.GetComponent<HomingMine>().Init(player.transform);
    //     else
    //         Debug.LogError("Failed to spawn mine or find Player tag.");
    // }

    IEnumerator Die()
    {
        // Play death animation, VFX, etc.
        Debug.Log("Boss defeated!");
        yield return new WaitForSeconds(2f);
        // Destroy the entire FinalBoss prefab root
        Destroy(transform.root.gameObject);

    }
}
