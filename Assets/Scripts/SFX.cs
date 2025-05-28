using UnityEngine;
using System;

public class SFX : MonoBehaviour
{
    public static SFX Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip deathSoundFX;
    [SerializeField] private AudioClip decrementLifeSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip victorySound;

    // Event that other scripts can subscribe to
    public event Action<string> OnSFXPlayed;

    private void Awake()
    {
        InitializeSingleton();
        ValidateAudioSources();
    }

    private void InitializeSingleton()
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

    private void ValidateAudioSources()
    {
        if (musicSource == null || sfxSource == null)
        {
            Debug.LogError("Audio sources not set in SFX manager!");
            enabled = false;
            return;
        }
    }

    private void PlaySound(AudioClip clip, string clipName)
    {
        if (clip == null)
        {
            Debug.LogWarning($"Attempted to play null audio clip: {clipName}");
            return;
        }

        sfxSource.PlayOneShot(clip);
        OnSFXPlayed?.Invoke(clipName);
    }

    // Public sound methods
    public void PlayDeathSound() => PlaySound(deathSoundFX, "Death");
    public void PlayDecrementLifeSound() => PlaySound(decrementLifeSound, "DecrementLife");
    public void PlayHitSound() => PlaySound(hitSound, "Hit");
    public void PlayGameOverSound() => PlaySound(gameOverSound, "GameOver");
    public void PlayVictorySound() => PlaySound(victorySound, "Victory");

    // Volume control methods
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = Mathf.Clamp01(volume);
    }
}
