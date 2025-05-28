using UnityEngine;
using System;

public class ParticleFX : MonoBehaviour
{
    public static ParticleFX Instance { get; private set; }

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem playerDamageVFX;
    [SerializeField] private ParticleSystem enemyDamageVFX;
    [SerializeField] private ParticleSystem playerDeathVFX;
    [SerializeField] private ParticleSystem enemyDeathVFX;

    // Events for VFX notifications
    public event Action<Vector3> OnPlayerDamageVFX;
    public event Action<Vector3> OnEnemyDamageVFX;
    public event Action<Vector3> OnPlayerDeathVFX;
    public event Action<Vector3> OnEnemyDeathVFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("ParticleFX Instance created successfully");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ValidateParticleSystems();
        SetupParticleSystems();
    }

    private void ValidateParticleSystems()
    {
        bool hasErrors = false;
        
        if (playerDamageVFX == null)
        {
            Debug.LogError("Player Damage VFX is not assigned!");
            hasErrors = true;
        }
        if (enemyDamageVFX == null)
        {
            Debug.LogError("Enemy Damage VFX is not assigned!");
            hasErrors = true;
        }
        if (playerDeathVFX == null)
        {
            Debug.LogError("Player Death VFX is not assigned!");
            hasErrors = true;
        }
        if (enemyDeathVFX == null)
        {
            Debug.LogError("Enemy Death VFX is not assigned!");
            hasErrors = true;
        }

        if (!hasErrors)
        {
            Debug.Log("All particle systems are properly assigned!");
        }
    }

    private void SetupParticleSystems()
    {
        // Ensure all particle systems are properly configured
        SetupParticleSystem(playerDamageVFX, "Player Damage VFX");
        SetupParticleSystem(enemyDamageVFX, "Enemy Damage VFX");
        SetupParticleSystem(playerDeathVFX, "Player Death VFX");
        SetupParticleSystem(enemyDeathVFX, "Enemy Death VFX");
    }

    private void SetupParticleSystem(ParticleSystem ps, string name)
    {
        if (ps == null) return;

        var main = ps.main;
        main.playOnAwake = false; // Ensure it doesn't play automatically
        main.loop = false; // One-shot effects
        
        // Stop any currently playing particles
        if (ps.isPlaying)
            ps.Stop();

        Debug.Log($"{name} configured successfully");
    }

    private void PlayEffect(ParticleSystem effect, Vector3 position, string effectName)
    {
        if (effect == null)
        {
            Debug.LogWarning($"Attempted to play null particle effect: {effectName}");
            return;
        }

        // Stop any currently playing effect
        if (effect.isPlaying)
            effect.Stop();

        // Set position and play
        effect.transform.position = position;
        effect.Play();
        
        Debug.Log($"Playing {effectName} effect at position: {position}");
    }

    public void PlayPlayerDamageVFX(Vector3 position)
    {
        PlayEffect(playerDamageVFX, position, "PlayerDamage");
        OnPlayerDamageVFX?.Invoke(position);
    }

    public void PlayEnemyDamageVFX(Vector3 position)
    {
        PlayEffect(enemyDamageVFX, position, "EnemyDamage");
        OnEnemyDamageVFX?.Invoke(position);
    }

    public void PlayPlayerDeathVFX(Vector3 position)
    {
        PlayEffect(playerDeathVFX, position, "PlayerDeath");
        OnPlayerDeathVFX?.Invoke(position);
    }

    public void PlayEnemyDeathVFX(Vector3 position)
    {
        PlayEffect(enemyDeathVFX, position, "EnemyDeath");
        OnEnemyDeathVFX?.Invoke(position);
    }
}