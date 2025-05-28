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

        if (playerStates.HealthBars <= 0)
        {
            playerStates.Die.Invoke();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
        timeManager.StopTimer();
        levelManager.DisplayGameOver();
    }
}
