using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	[Header("Pause Menu")]
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private Button resumeButton;

	private StandaloneInputModule inputModule;

	public GameState GameState { get; set; }
	public int PNumberOnPause { get; private set; }

	public static GameManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		else Instance = this;

		DontDestroyOnLoad(gameObject);

		inputModule = FindObjectOfType<StandaloneInputModule>();
	}

	private void Start()
	{
		GameState = GameState.Match;
		PNumberOnPause = 1;
	}

	// Update is called once per frame
	private void Update()
    {
		if(GameState == GameState.Match) PauseGame();
    }

	private void PauseGame()
	{
		CheckPauseForPlayer(1);
		CheckPauseForPlayer(2);
	}

	private void CheckPauseForPlayer(int pNumber)
	{

		if (InputManager.GetButtonDown(pNumber, "Pause") && GameState == GameState.Match)
		{
			if (GameState != GameState.PauseMenu)
			{
				GameState = GameState.PauseMenu;
				pauseMenu.SetActive(true);
				SetEventSystemInputModule(pNumber);
				PNumberOnPause = pNumber;
				resumeButton.Select();
				resumeButton.OnSelect(null);
			}
		}
	}

	private void SetEventSystemInputModule(int pNumber)
	{
		inputModule.horizontalAxis = $"P{pNumber} Menu Control Horizontal";
		inputModule.verticalAxis = $"P{pNumber} Menu Control Vertical";
		inputModule.submitButton = $"P{pNumber} Choose";
		inputModule.cancelButton = $"P{pNumber} Cancel";
	}
}
