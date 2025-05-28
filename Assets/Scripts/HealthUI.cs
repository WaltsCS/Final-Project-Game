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
        playerStates.Die += EmptyHealthUI;

    }

    void OnDisable()
    {
        playerStates.TakeDamage -= UpdateHealthUI;
        playerStates.ReplenishHealth -= UpdateHealthUI;
        playerStates.Die -= EmptyHealthUI;
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

    void EmptyHealthUI()
    {
        foreach (var bar in healthBars)
        {
            bar.enabled = false;
        }
    }
}