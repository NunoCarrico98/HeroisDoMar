using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeTime;

    public static SoundManager Instance { get; set; }

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
        float startVolume = musicSource.volume;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    public IEnumerator MusicPartialFadeOut(float finalVolume)
    {
        float startVolume = musicSource.volume;
        finalVolume = Mathf.Clamp(finalVolume, 0, 100);
        finalVolume /= 100;
        Debug.Log(finalVolume);

        while (musicSource.volume > finalVolume)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}
