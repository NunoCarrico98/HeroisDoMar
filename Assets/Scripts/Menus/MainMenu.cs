using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private float textFadeDuration;
	[SerializeField] private float buttonFadeDuration;
	[SerializeField] private float imageFadeDuration;

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
	[SerializeField] private GameObject settingsContent;
	//[SerializeField] private Graphic[] settingsContentUIElements;

	[Header("Quit")]
	[SerializeField] private GameObject quitCheck;
	[SerializeField] private Button yesButton;

	private UIManager uiManager;
	private bool playFlag;
	private bool collectionFlag;
	private bool settingsFlag;

	private void Awake()
	{
		playFlag = false;
		collectionFlag = false;
		settingsFlag = false;

		uiManager = FindObjectOfType<UIManager>();
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
			Debug.Log(EventSystem.current.currentSelectedGameObject.name);
			EnableMenuContent(true, false, false);
			//FadeUI(playContenttUIElements, false);
		}
		else if (EventSystem.current.currentSelectedGameObject == collectionButton.gameObject && !collectionFlag)
		{
			Debug.Log(EventSystem.current.currentSelectedGameObject.name);
			EnableMenuContent(false, true, false);
			//FadeUI(collectionContenttUIElements, false);
		}
		else if (EventSystem.current.currentSelectedGameObject == settingsButton.gameObject && !settingsFlag)
		{
			Debug.Log(EventSystem.current.currentSelectedGameObject.name);
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
