using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    private PlayerStates playerStates;
    private LevelManager levelManager;
    private TimeManager timeManager;

    private void Awake()
    {
        playerStates = GetComponent<PlayerStates>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
    }

    private void OnEnable()
    {
        playerStates.TakeDamage += TakeDamage;
        playerStates.Die += Die;
    }

    private void OnDisable()
    {
        playerStates.TakeDamage -= TakeDamage;
        playerStates.Die -= Die;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {
            playerStates.TakeDamage.Invoke();
        }
    }

    private void TakeDamage()
    {
        if (!levelManager.IsLevelActive) return;

        playerStates.HealthBars--;
        if (SFX.Instance != null)
            SFX.Instance.PlayDecrementLifeSound();
        
        Debug.Log($"Player took damage. Remaining health bars: {playerStates.HealthBars}");
        
        // Fixed: Use PlayPlayerDamageVFX instead of PlayEnemyDamageVFX
        if (ParticleFX.Instance != null)
        {
            Debug.Log("Playing player damage VFX...");
            ParticleFX.Instance.PlayPlayerDamageVFX(transform.position);
        }
        else
        {
            Debug.LogWarning("ParticleFX.Instance is null!");
        }
        
        if (playerStates.HealthBars <= 0)
        {
            // Play death VFX before destroying
            if (ParticleFX.Instance != null)
            {
                ParticleFX.Instance.PlayPlayerDeathVFX(transform.position);
            }
            playerStates.Die.Invoke();
        }
    }

    private void Die()
    {
        // Small delay to allow death VFX to play

        if (ParticleFX.Instance != null)
        {
            Debug.Log("Playing player damage VFX...");
            ParticleFX.Instance.PlayPlayerDeathVFX(transform.position);
        }
        Invoke(nameof(DestroyPlayer), 0.1f);
        timeManager.StopTimer();
        levelManager.DisplayGameOver();

    }

    private void DestroyPlayer()
    {
        Destroy(this.gameObject);
    }
}