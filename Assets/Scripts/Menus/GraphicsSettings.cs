using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
	[SerializeField] Vector2[] resolutions;
	[SerializeField] TMP_Dropdown dropdownResolutions;

	private void Start()
	{
		SetDropdownScreenResolutionsOptions();
	}

	public void SetFullscreenSetting()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void SetScreenResolutionSetting()
	{
		Screen.SetResolution((int)resolutions[dropdownResolutions.value].x,
				(int)resolutions[dropdownResolutions.value].y, Screen.fullScreen);
		Debug.Log(Screen.currentResolution);
	}

	private void SetDropdownScreenResolutionsOptions()
	{
		for (int i = 0; i < resolutions.Length; i++)
		{
			dropdownResolutions.options.Add(new TMP_Dropdown.OptionData());
			dropdownResolutions.options[i].text = resolutions[i].x + "x" + resolutions[i].y;
			dropdownResolutions.value = i;
		}
	}
}
