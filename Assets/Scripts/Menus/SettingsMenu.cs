using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
	[Header("General")]
	[SerializeField] private Button generalButton;
	[SerializeField] private GameObject generalContent;
	[SerializeField] private TextMeshProUGUI generalTitle; 

	[Header("Graphics")]
	[SerializeField] private Button graphicsButton;
	[SerializeField] private GameObject graphicsContent;
	[SerializeField] private TextMeshProUGUI graphicsTitle;

	[Header("Sound")]
	[SerializeField] private Button soundButton;
	[SerializeField] private GameObject soundContent;
	[SerializeField] private TextMeshProUGUI soundTitle;

	[Header("Back")]
	[SerializeField] private Button backButton;
	[SerializeField] private Button selectAfterBack;
	[SerializeField] private GameObject settingsMenu;
	[SerializeField] private TextMeshProUGUI backText;

	private bool generalFlag;
	private bool graphicsFlag;
	private bool soundFlag;
	private bool backFlag;

	private void Update()
	{
		CheckSelectedButton();
	}

	private void IsLeavingByButton()
	{

	}

	private void CheckSelectedButton()
	{
		GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
		if (currentSelected == generalButton.gameObject && !generalFlag)
		{
			ChangeTitleColors(Color.black, Color.white, Color.white, Color.white);
			EnableSettingsOptionContent(true, false, false);
		}
		else if (currentSelected == graphicsButton.gameObject && !graphicsFlag)
		{
			ChangeTitleColors(Color.white, Color.black, Color.white, Color.white);
			EnableSettingsOptionContent(false, true, false);
		}
		else if (currentSelected == soundButton.gameObject && !soundFlag)
		{
			ChangeTitleColors(Color.white, Color.white, Color.black, Color.white);
			EnableSettingsOptionContent(false, false, true);
		}
		else if (currentSelected == backButton.gameObject && !backFlag)
		{
			ChangeTitleColors(Color.white, Color.white, Color.white, Color.black);
			FlagsToFalse();
		}
		else if (currentSelected != generalButton.gameObject &&
				currentSelected != graphicsButton.gameObject &&
				currentSelected != soundButton.gameObject &&
				currentSelected != backButton.gameObject)
		{
			ChangeTitleColors(Color.white, Color.white, Color.white, Color.white);
			FlagsToFalse();
		}
	}

	private void ChangeTitleColors(Color generalColor, Color graphicsColor, Color soundColor, Color backColor)
	{
		generalTitle.color = generalColor;
		graphicsTitle.color = graphicsColor;
		soundTitle.color = soundColor;
		backText.color = backColor;
	}

	private void EnableSettingsOptionContent(bool generalFlag0, bool graphicsFlag0, bool soundFlag0)
	{
		generalFlag = generalFlag0;
		graphicsFlag = graphicsFlag0;
		soundFlag = soundFlag0;

		generalContent.SetActive(generalFlag0);
		graphicsContent.SetActive(graphicsFlag0);
		soundContent.SetActive(soundFlag0);
	}

	private void FlagsToFalse()
	{
		generalFlag = false;
		graphicsFlag = false;
		soundFlag = false;
	}

	public void ClickBack()
	{
		settingsMenu.SetActive(false);
		selectAfterBack.Select();
		selectAfterBack.OnSelect(null);
	}
}
