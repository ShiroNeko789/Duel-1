using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip currentSFXClip;

    public AudioSource bgmSource;  // AudioSource for background music
    public AudioSource sfxSource;  // AudioSource for sound effects
    public AudioClip[] bgmClips;   // Array for background music clips
    public AudioClip[] sfxClips;   // Array for sound effect clips

    private void Awake()
    {
        // Make sure there is only one instance of AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep AudioManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Ensure audio sources are assigned, if not, add them.
        if (bgmSource == null)
            bgmSource = gameObject.AddComponent<AudioSource>();

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBGM(int index)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM index out of range!");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            currentSFXClip = sfxClips[index];
            sfxSource.PlayOneShot(currentSFXClip);
        }
        else
        {
            Debug.LogWarning("SFX index out of range!");
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
