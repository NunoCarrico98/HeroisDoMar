using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerHandler : MonoBehaviour
{
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public void PlaySFX(AudioClip sfx)
    {
        soundManager.PlaySFX(sfx);
    }
}
