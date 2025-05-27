using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    [Header("Fire Rate Properties")]
    [SerializeField] private float fireRateIncreaseDuration = 5f;
    [SerializeField] private float fireRateValue = 0.05f;
    [SerializeField] private PowerUpUI powerUpUI;

    private PlayerStates playerStates;
    private Coroutine fireRateCoroutine;

    void Awake()
    {
        playerStates = GetComponent<PlayerStates>();
    }

    private void OnEnable()
    {
        playerStates.IncreaseFireRate += IncreaseFireRate;
        playerStates.ReplenishHealth += ReplenishHealth;
    }

    private void OnDisable()
    {
        playerStates.IncreaseFireRate -= IncreaseFireRate;
        playerStates.ReplenishHealth -= ReplenishHealth;
    }

    private void IncreaseFireRate()
    {
        if (fireRateCoroutine != null)
        {
            StopCoroutine(fireRateCoroutine);
        }

        fireRateCoroutine = StartCoroutine(FireRateTimer());

        if (powerUpUI != null)
            powerUpUI.ShowPowerUp(fireRateIncreaseDuration);
    }

    private IEnumerator FireRateTimer()
    {
        float originalFireRate = playerStates.FireRate;
        playerStates.FireRate = fireRateValue;
        yield return new WaitForSeconds(fireRateIncreaseDuration);
        playerStates.FireRate = originalFireRate;
        fireRateCoroutine = null;
    }

    private void ReplenishHealth()
    {
        if (playerStates.HealthBars < 3)
        {
            playerStates.HealthBars++;
            Debug.Log("Health replenished. Current health bars: " + playerStates.HealthBars);
        }
    }
}
