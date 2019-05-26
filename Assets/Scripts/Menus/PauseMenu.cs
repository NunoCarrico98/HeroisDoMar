using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Button settingsGeneralButton;
	[SerializeField] private Button settingsGraphicsButton;
	[SerializeField] private Button settingsSoundButton;
	[SerializeField] private GameObject settingsMenu;
	[SerializeField] private GameObject settingsGeneralContent;
	[SerializeField] private GameObject settingsGraphicsContent;
	[SerializeField] private GameObject settingsSoundContent;

	[Header("Leave Game")]
	[SerializeField] private GameObject leaveGameCheck;
	[SerializeField] private Button leaveGameButton;

	private GameManager gameManager;

	private void Awake()
	{
		gameManager = GameManager.Instance;
	}

	private void Update()
	{
		IsLeavingByButton();
	}

	private void IsLeavingByButton()
	{
		if (InputManager.GetButtonDown(gameManager.PNumberOnPause, "Pause"))
		{
			if (gameManager.GameState == GameState.PauseMenu)
				ClickResume();
			if (gameManager.GameState == GameState.QuitCheck)
				ClickLeaveGameNo();
		}
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

	public void ClickSettings()
	{
		gameManager.GameState = GameState.SettingsMenu;
		EnableSettingsMenuContent(true, false, false, settingsGraphicsButton);
	}

	public void ClickBackOnSettings(Button selectAfterBack)
	{
		gameManager.GameState = GameState.PauseMenu;
		selectAfterBack.Select();
		selectAfterBack.OnSelect(null);
		settingsMenu.SetActive(false);
	}

	public void ClickResume()
	{
		gameManager.GameState = GameState.Match;
		gameObject.SetActive(false);
	}

	public void ClickLeaveGame(Button yesButton)
	{
		gameManager.GameState = GameState.QuitCheck;
		leaveGameCheck.SetActive(true);
		yesButton.Select();
		yesButton.OnSelect(null);
	}

	public void ClickLeaveGameYes() => SceneManager.LoadScene("MainMenu");

	public void ClickLeaveGameNo()
	{
		gameManager.GameState = GameState.PauseMenu;
		leaveGameCheck.SetActive(false);
		leaveGameButton.Select();
		leaveGameButton.OnSelect(null);
	}
}
