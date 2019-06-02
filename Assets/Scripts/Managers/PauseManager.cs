using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
	[Header("Pause Menu")]
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private Button resumeButton;

	private StandaloneInputModule inputModule;
	private GameManager gameManager;

	public int PNumberOnPause { get; private set; }

	private void Awake()
	{
		gameManager = GameManager.Instance;
		inputModule = FindObjectOfType<StandaloneInputModule>();
	}

	private void Start()
	{
		PNumberOnPause = 1;
		gameManager.ActivateCameraController(true);
	}

	private void Update()
	{
		PauseGame();
	}

	private void PauseGame()
	{
		CheckPauseForPlayer(1);
		CheckPauseForPlayer(2);
	}

	private void CheckPauseForPlayer(int pNumber)
	{

		if (InputManager.GetButtonDown(pNumber, "Pause") &&
			gameManager.GameState == GameState.Match)
		{
			if (gameManager.GameState != GameState.PauseMenu)
			{
				gameManager.GameState = GameState.PauseMenu;
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
