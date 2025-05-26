using UnityEngine;
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

    private float shootTimer;
    private List<Transform> shootPoints;

    private void Awake()
    {
        ///Gather all child shoot points
        if (shootPointsRoot != null)
            shootPoints = new List<Transform>(shootPointsRoot.childCount);
        else
            Debug.LogError("[EnemyFinalBoss] ShootPointsRoot not assigned.");

        foreach (Transform child in shootPointsRoot)
            shootPoints.Add(child);

        shootTimer = shootInterval;
    }

    private void Update()
    {
        ///Rotate the shoot points around the boss
        if (shootPointsRoot != null)
            shootPointsRoot.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);

        ///Shooting logic
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            FireAll();
            shootTimer = shootInterval;
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
}

