using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
	[Header("Dropdown")]
	[SerializeField] Vector2[] resolutions;
	[SerializeField] TMP_Dropdown dropdownResolutions;
	[SerializeField] TMP_Dropdown dropdownQuality;


	private	PostProcessVolume ppProfile;
	private PostProcessLayer ppLayer;
	private AmbientOcclusion aoLayer;
	private Bloom bloomLayer;

	private void Awake()
	{
		ppProfile = FindObjectOfType<PostProcessVolume>();
		ppLayer = FindObjectOfType<PostProcessLayer>();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += SetPostProcess;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= SetPostProcess;
	}

	private void Start()
	{
		SetDropdownScreenResolutionsOptions();
		SetDropdownQualityOptions();
	}

	public void SetFullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void SetScreenResolution()
	{
		Screen.SetResolution((int)resolutions[dropdownResolutions.value].x,
				(int)resolutions[dropdownResolutions.value].y, Screen.fullScreen);
	}

	public void SetQuality()
	{
		QualitySettings.SetQualityLevel(dropdownQuality.value, true);
	}

	public void SetVSync(Toggle toggle)
	{
		if (toggle.isOn) QualitySettings.vSyncCount = 1;
		else QualitySettings.vSyncCount = 0;
	}

	public void SetAA(Toggle toggle)
	{
		if(toggle.isOn) ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
		else ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
	}

	public void SetAmbientOcclusion()
	{
		if(ppProfile != null) ppProfile.profile.TryGetSettings(out aoLayer);
		aoLayer.enabled.value = !aoLayer.enabled.value;
	}

	public void SetShadows(Toggle toggle)
	{
		if (toggle.isOn) QualitySettings.shadows = ShadowQuality.All;
		else QualitySettings.shadows = ShadowQuality.Disable;
	}

	public void SetSoftParticles()
	{
		QualitySettings.softParticles = !QualitySettings.softParticles;
	}

	public void SetBloom()
	{
		if (ppProfile != null) ppProfile.profile.TryGetSettings(out bloomLayer);
		bloomLayer.enabled.value = !bloomLayer.enabled.value;
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

	private void SetDropdownQualityOptions()
	{
		for (int i = 0; i < QualitySettings.names.Length; i++)
		{
			dropdownQuality.options.Add(new TMP_Dropdown.OptionData());
			dropdownQuality.options[i].text = QualitySettings.names[i];
			dropdownQuality.value = i;
		}
	}

	public void SetPostProcess(Scene scene, LoadSceneMode mode)
	{
		ppProfile = FindObjectOfType<PostProcessVolume>();
		ppLayer = FindObjectOfType<PostProcessLayer>();
	}
}
