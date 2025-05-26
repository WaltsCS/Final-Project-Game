using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    private PlayerStates playerStates;

    private void Awake()
    {
        playerStates = GetComponent<PlayerStates>();
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            playerStates.TakeDamage.Invoke();
        }
    }

    private void TakeDamage()
    {
        if (!playerStates.IsAlive) return;

        playerStates.HealthBars--;
        Debug.Log($"Player took damage. Remaining health bars: {playerStates.HealthBars}");

        if (playerStates.HealthBars <= 0)
        {
            playerStates.Die.Invoke();
        }
    }

    private void Die()
    {
        playerStates.IsAlive = false;
        GameObject.Find("GameManager").GetComponent<GameManager>().DisplayGameOver();
    }
}
