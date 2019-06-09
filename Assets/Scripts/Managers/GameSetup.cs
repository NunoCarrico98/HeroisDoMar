using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameSetup : MonoBehaviour
{
	[Header("Player")]
	[SerializeField] private Image[] selectedHeroesImages;
	[SerializeField] private TextMeshProUGUI[] selectedHeroesText;
	[SerializeField] private int maxPlayersNumb;
	[SerializeField] private Color[] playerColors;
	[SerializeField] private GameObject[] playerControlsButtons;
	[SerializeField] private GameObject[] playerReadyPanels;
	[SerializeField] private GameObject[] playerReadyButtons;

	[Header("Event Systems")]
	[SerializeField] private Button initialP1Button;
	[SerializeField] private Button initialP2Button;
	[SerializeField] private EventSystem[] playersES;

	[Header("Match Countdown")]
	[SerializeField] private GameObject matchCountdown;
	[SerializeField] private int waitTime;
	[SerializeField] private TextMeshProUGUI waitTimeText;

	private int choosingPlayer;
	private GameObject[] lastCharacterSelected;
	private CharacterSelectState[] playerState;
	private bool[] playersActive;
	private bool hasGameStarted;
	private GameManager gameManager;
	private int currentTime;

	private void Awake()
	{
		gameManager = GameManager.Instance;
	}

	private void Start()
	{
		playerState = new CharacterSelectState[maxPlayersNumb];
		lastCharacterSelected = new GameObject[maxPlayersNumb];
		playersActive = new bool[maxPlayersNumb];
		hasGameStarted = false;
		currentTime = waitTime;
	}

	// Update is called once per frame
	private void Update()
	{
		if (gameManager.GameState == GameState.CharacterSelect)
		{
			SetPlayers();
			StartGame();
		}
		GoBackByButton(1);
		GoBackByButton(2);
		//GoBackByButton(3);
		//GoBackByButton(4);
	}

	private void GoBackByButton(int playerNumber)
	{
		if (InputManager.GetButtonDown(playerNumber, "Cancel"))
		{
			int player = playerNumber - 1;
			if (playerState[player] == CharacterSelectState.Chosen)
			{
				playerState[player] = CharacterSelectState.Choosing;
				playersES[player].SetSelectedGameObject(lastCharacterSelected[player]);
			}
			if (playerState[player] == CharacterSelectState.Ready)
			{
				playerState[player] = CharacterSelectState.Chosen;
				playerReadyPanels[player].SetActive(false);
				playersES[player].SetSelectedGameObject(playerReadyButtons[player]);
			}
			if (gameManager.GameState == GameState.Match)
				CancelMatch(player);
		}
	}

	private void SetPlayers()
	{
		for (int i = 0; i < maxPlayersNumb; i++)
		{
			int pNumb = i + 1;
			if (!playersActive[i])
			{
				if (InputManager.GetButtonDown(pNumb, "Choose"))
				{
					playersActive[i] = true;
					playerState[i] = CharacterSelectState.Choosing;

					gameManager.CreateNewPlayer(pNumb, playerColors[i]);

					SetPlayerSelectionControls(pNumb);
					SetSelectedInitialButton(pNumb);
				}
			}
		}
	}

	private void SetPlayerSelectionControls(int pNumb)
	{
		StandaloneInputModule inputModule = playersES[pNumb - 1].GetComponent<StandaloneInputModule>();

		inputModule.horizontalAxis = $"P{pNumb} Menu Control Horizontal";
		inputModule.verticalAxis = $"P{pNumb} Menu Control Vertical";
		inputModule.submitButton = $"P{pNumb} Choose";
		inputModule.cancelButton = $"P{pNumb} Cancel";
	}

	private void SetSelectedInitialButton(int pNumb)
	{
		switch (pNumb)
		{
			case 1:
				initialP1Button.Select();
				break;
			case 2:
				initialP2Button.Select();
				break;
		}
	}

	public void SetChoosingPlayer(int player) => choosingPlayer = player;

	public void ChooseCharacter(GameObject hero)
	{
		int player = choosingPlayer - 1;
		playerState[player] = CharacterSelectState.Chosen;
		gameManager.Players[player].SetPlayerCharacter(hero);
		UpdateText(hero);
		SetButtonAfterSelection(player);
	}

	private void SetButtonAfterSelection(int player)
	{
		lastCharacterSelected[player] = playersES[player].currentSelectedGameObject;
		playersES[player].SetSelectedGameObject(playerControlsButtons[player]);
	}

	public void SetPlayerReady()
	{
		int player = choosingPlayer - 1;
		playerState[player] = CharacterSelectState.Ready;
		playersES[player].SetSelectedGameObject(null);
		playerReadyPanels[player].SetActive(true);
	}

	private void UpdateText(GameObject hero)
	{
		selectedHeroesImages[choosingPlayer - 1].sprite = hero.GetComponent<Hero>().SelectionScreenBackground;
		selectedHeroesText[choosingPlayer - 1].text = $"PLAYER {choosingPlayer} \n\n {hero.name}";
	}

	private void StartGame()
	{
		if (playerState.All(pState => pState == CharacterSelectState.Ready))
		{
			gameManager.GameState = GameState.Match;
			matchCountdown.SetActive(true);
			InvokeRepeating("CountdownToMatch", 0, 1);
		}
	}

	private void CountdownToMatch()
	{
		if (currentTime == 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		waitTimeText.text = currentTime.ToString();
		currentTime--;
	}

	private void CancelMatch(int player)
	{
		gameManager.GameState = GameState.CharacterSelect;
		CancelInvoke("CountdownToMatch");
		matchCountdown.SetActive(false);
		currentTime = waitTime;

		for (int i = 0; i < playerState.Length; i++)
			playerState[i] = CharacterSelectState.Chosen;
		for (int i = 0; i < playersES.Length; i++)
			playersES[i].SetSelectedGameObject(playerReadyButtons[i]);
		foreach (GameObject panel in playerReadyPanels)
			panel.SetActive(false);
	}
}
