using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootSoundFX;
    [SerializeField] private AudioClip deathSoundFX;
    [SerializeField] private AudioClip takeDamageSoundFX;


    private AudioSource audioSource;
    private LevelManager levelManager;
    private PlayerStates playerStates;

    void Awake()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        playerStates = GameObject.Find("Player").GetComponent<PlayerStates>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerStates.Shoot += PlayShootSound;
        playerStates.TakeDamage += PlayTakeDamageSound;
        playerStates.Die += PlayDeathSound;
    }

    private void OnDisable()
    {
        playerStates.Shoot -= PlayShootSound;
        playerStates.TakeDamage -= PlayTakeDamageSound;
        playerStates.Die -= PlayDeathSound;
    }

    private void PlayShootSound()
    {
        if (levelManager.IsLevelActive)
        {
            audioSource.PlayOneShot(shootSoundFX);
        }
    }

    private void PlayTakeDamageSound()
    {
        if (levelManager.IsLevelActive)
        {
            audioSource.PlayOneShot(takeDamageSoundFX);

        }
    }

    private void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSoundFX);
    }

}
