using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour
{
    [Header("Power-Up Properties")]
    [SerializeField] private float expirationTime = 5f;
    [SerializeField] private float rotationSpeed = 50f;

    public GameObject powerUpSpawnPoint;

    private Coroutine stayUpTimerCoroutine;
    private PowerUpStates powerUpStates;
    private GameManager gameManager;

    void Awake()
    {
        powerUpStates = GetComponent<PowerUpStates>();
    }

    private void OnEnable()
    {
        powerUpStates.OnPowerUpPopUp += ShowPowerUp;
        powerUpStates.OnPowerUpExpire += HidePowerUp;
        powerUpStates.OnPlayerHit += HidePowerUp;
    }

    private void OnDisable()
    {
        powerUpStates.OnPowerUpPopUp -= ShowPowerUp;
        powerUpStates.OnPowerUpExpire -= HidePowerUp;
        powerUpStates.OnPlayerHit -= HidePowerUp;
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        powerUpStates.OnPowerUpPopUp.Invoke();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        PlayerStates playerStates = other.gameObject.GetComponent<PlayerStates>();
        if (other.gameObject.CompareTag("Player") && playerStates != null)
        {
            if (gameObject.CompareTag("FireRatePowerUp"))
            {
                playerStates.IncreaseFireRate.Invoke();
            }
            else if (gameObject.CompareTag("HealthPowerUp"))
            {
                playerStates.ReplenishHealth.Invoke();
            }
            powerUpStates.OnPlayerHit.Invoke();
        }
    }

    private IEnumerator StayUpTimer()
    {
        yield return new WaitForSeconds(expirationTime);
        powerUpStates.OnPowerUpExpire.Invoke();
    }

    private void ShowPowerUp()
    {
        if (gameManager.IsGameActive)
        {
            stayUpTimerCoroutine = StartCoroutine(StayUpTimer());
        }
    }

    private void HidePowerUp()
    {
        if (stayUpTimerCoroutine != null)
        {
            StopCoroutine(stayUpTimerCoroutine);
        }
        Destroy(gameObject);
    }
}
