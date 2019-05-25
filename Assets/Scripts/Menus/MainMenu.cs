using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
	[Header("Play")]
	[SerializeField] private Button playButton;
	[SerializeField] private GameObject playContent;
	//[SerializeField] private Graphic[] playContentUIElements;

	[Header("Collection")]
	[SerializeField] private Button collectionButton;
	[SerializeField] private GameObject collectionContent;
	//[SerializeField] private Graphic[] collectionContenttUIElements;

	[Header("Settings")]
	[SerializeField] private Button settingsButton;
	[SerializeField] private Button settingsGeneralButton;
	[SerializeField] private Button settingsGraphicsButton;
	[SerializeField] private Button settingsSoundButton;
	[SerializeField] private Button settingsGeneralButton2;
	[SerializeField] private Button settingsGraphicsButton2;
	[SerializeField] private Button settingsSoundButton2;
	[SerializeField] private GameObject settingsContent;
	[SerializeField] private GameObject settingsMenu;
	[SerializeField] private GameObject settingsGeneralContent;
	[SerializeField] private GameObject settingsGraphicsContent;
	[SerializeField] private GameObject settingsSoundContent;
	//[SerializeField] private Graphic[] settingsContentUIElements;

	[Header("Quit")]
	[SerializeField] private GameObject quitCheck;
	[SerializeField] private Button yesButton;

	private UIManager uiManager;
	private GameManager gameManager;

	private bool playFlag;
	private bool collectionFlag;
	private bool settingsFlag;

	private void Awake()
	{
		playFlag = false;
		collectionFlag = false;
		settingsFlag = false;

		uiManager = FindObjectOfType<UIManager>();
		gameManager = FindObjectOfType<GameManager>();
	}

	private void Start()
	{
		DisableMouse();
	}

	private void DisableMouse()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		CheckSelectedButton();
	}

	private void CheckSelectedButton()
	{
		if (EventSystem.current.currentSelectedGameObject == playButton.gameObject && !playFlag)
		{
			EnableMenuContent(true, false, false);
			//FadeUI(playContenttUIElements, false);
		}
		else if (EventSystem.current.currentSelectedGameObject == collectionButton.gameObject && !collectionFlag)
		{
			EnableMenuContent(false, true, false);
			//FadeUI(collectionContenttUIElements, false);
		}
		else if (EventSystem.current.currentSelectedGameObject == settingsButton.gameObject && !settingsFlag)
		{
			EnableMenuContent(false, false, true);
			//FadeUI(settingsContentUIElements, false);
		}
	}

	private void EnableMenuContent(bool playFlag0, bool collectionFlag0, bool settingsFlag0)
	{
		playFlag = playFlag0;
		collectionFlag = collectionFlag0;
		settingsFlag = settingsFlag0;

		playContent.SetActive(playFlag0);
		collectionContent.SetActive(collectionFlag0);
		settingsContent.SetActive(settingsFlag0);
	}

	private void EnableSettingsMenuContent(bool settingsGeneralFlag0, bool settingsGraphicsFlag0, 
		bool settingsSoundFlag0, Button button)
	{
		settingsMenu.SetActive(true);
		settingsGeneralContent.SetActive(settingsGeneralFlag0);
		settingsGraphicsContent.SetActive(settingsGraphicsFlag0);
		settingsSoundContent.SetActive(settingsSoundFlag0);

		button.Select();
		button.OnSelect(null);
	}

	public void ClickGeneralSettings() 
		=> EnableSettingsMenuContent(true, false, false, settingsGeneralButton2);

	public void ClickGraphicsSettings()
		=> EnableSettingsMenuContent(false, true, false, settingsGraphicsButton2);

	public void ClickSoundSettings()
		=> EnableSettingsMenuContent(false, false, true, settingsSoundButton2);

	public void QuitGame()
	{
		Debug.Log("Clicked");
		quitCheck.SetActive(true);
		yesButton.Select();
		yesButton.OnSelect(null);
	}

	public void QuitGameYes()
	{
		Application.Quit();
	}

	public void QuitGameNo()
	{
		quitCheck.SetActive(false);
		playButton.Select();
		playButton.OnSelect(null);
	}

	/*private void FadeUI(Graphic[] uiElements, bool fadeOut)
	{
		foreach (Graphic ui in uiElements)
			uiManager.FadeUiElement(ui, fadeOut, buttonFadeDuration);
	}*/

}
