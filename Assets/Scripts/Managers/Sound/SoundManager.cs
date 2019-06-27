using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeTime;
    [Header("Music")]
    [SerializeField] private AudioClip mainMenuTheme;
    [SerializeField] private AudioClip matchTheme;
    [Header("Sound Effects")]
    [SerializeField] private AudioClip victorySFX;

    public static SoundManager Instance { get; set; }

    private bool isFading;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this || Instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        volume = Mathf.Clamp(volume, 0, 100);
        volume /= 100;
        sfxSource.volume = volume;

        sfxSource.PlayOneShot(clip);
    }

    public IEnumerator MusicFadeOut()
    {
        isFading = true;

        float startVolume = musicSource.volume;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = startVolume;
        musicSource.clip = null;

        isFading = false;
    }

    public IEnumerator MusicPartialFadeOut(float finalVolume)
    {
        isFading = true;

        float startVolume = musicSource.volume;
        finalVolume = Mathf.Clamp(finalVolume, 0, 100);
        finalVolume /= 100;
        Debug.Log(finalVolume);

        while (musicSource.volume > finalVolume)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        isFading = false;
    }

    public void StateChangeAudioHandler(GameState gs)
    {
        Debug.Log(gs);
        switch (gs)
        {
            case GameState.MainMenu:
                if (sfxSource.isPlaying) sfxSource.Stop();
                if (musicSource.clip != mainMenuTheme)
                {
                    musicSource.volume = 1f;
                    musicSource.clip = mainMenuTheme;
                    musicSource.Play();
                }
                break;
            case GameState.Match:
                if (sfxSource.isPlaying && sfxSource.clip == victorySFX) sfxSource.Stop();
                if (!musicSource.isPlaying) musicSource.Play();
                if (musicSource.clip != matchTheme)
                {
                    StartCoroutine(MusicFadeOut());
                    StartCoroutine(PlayAfterFade(matchTheme, 15f));
                }
                break;
            case GameState.VictoryScreen:
                musicSource.Stop();
                sfxSource.clip = victorySFX;
                sfxSource.Play();
                break;
        }
    }

    private IEnumerator PlayAfterFade(AudioClip clip, float volume)
    {
        volume = Mathf.Clamp(volume, 0, 100);
        volume /= 100;

        while (isFading)
            yield return null;
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }
}
