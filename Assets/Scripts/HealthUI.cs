using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerStates playerStates;
    [SerializeField] private Image[] healthBars;

    void OnEnable()
    {
        playerStates.TakeDamage += UpdateHealthUI;
        playerStates.ReplenishHealth += UpdateHealthUI;
        playerStates.Die += UpdateHealthUI;
    }

    void OnDisable()
    {
        playerStates.TakeDamage -= UpdateHealthUI;
        playerStates.ReplenishHealth -= UpdateHealthUI;
        playerStates.Die -= UpdateHealthUI; 
    }

    void Start()
    {
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        int currentHealth = playerStates.HealthBars;

        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].enabled = i < currentHealth;
        }
    }
}