using UnityEngine;
using System;

public class SFX : MonoBehaviour
{
    public static SFX Instance { get; private set; }

    // Events for sound notifications
    public static event Action OnDeathSound;
    public static event Action OnCollectibleSound;
    public static event Action OnGameOverSound;
    public static event Action OnVictorySound;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip deathSoundFX;
    [SerializeField] private AudioClip collectibleSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip victorySound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDeathSound()
    {
        sfxSource.PlayOneShot(deathSoundFX);
        OnDeathSound?.Invoke();
    }

    public void PlayCollectibleSound()
    {
        sfxSource.PlayOneShot(collectibleSound);
        OnCollectibleSound?.Invoke();
    }

    public void PlayGameOverSound()
    {
        sfxSource.PlayOneShot(gameOverSound);
        OnGameOverSound?.Invoke();
    }

    public void PlayVictorySound()
    {
        sfxSource.PlayOneShot(victorySound);
        OnVictorySound?.Invoke();
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
