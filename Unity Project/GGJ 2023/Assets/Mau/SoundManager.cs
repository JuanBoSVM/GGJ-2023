using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AUDIOSOURCES
{
    Music,
    SFX
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Source References")]
    public AudioSource m_musicAudioSource;
    public AudioSource m_sfxAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioSource _audioSource, AudioClip _clip, float _volume = 1.0f)
    {
        _audioSource.PlayOneShot(_clip, _volume);
    }

    //play or change current music
    public void PlayMusic(AudioClip clip, float _volume = 1.0f)
    {
        m_musicAudioSource.Stop();
        m_musicAudioSource.volume = _volume;
        m_musicAudioSource.clip = clip;
        m_musicAudioSource.time = 0.0f; 
        m_musicAudioSource.Play();
    }

    //play or change current ambient clip
    public void PlaySFX(AudioClip clip, float _volume = 1.0f)
    {
        m_sfxAudioSource.Stop();
        m_sfxAudioSource.volume = _volume;
        m_sfxAudioSource.clip = clip;
        m_sfxAudioSource.Play();
    }
}
