using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{
	public void SetMasterSound()
	{
		AudioListener.pause = !AudioListener.pause;
	}

	public void SetMusicSound()
	{

	}

	public void SetVFXSound()
	{

	}
}
