using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameSetup : MonoBehaviour
{
	[Header("Player")]
	[SerializeField] private Image[] selectedHeroes;
	[SerializeField] private int maxPlayersNumb;
	[SerializeField] private Color[] playerColors;
	[SerializeField] private GameObject[] allHeroes;

	[Header("Buttons")]
	[SerializeField] private Button initialP1Button;
	[SerializeField] private Button initialP2Button;
	[SerializeField] private EventSystem[] playersES;

	private GameManager gameManager;
	private int choosingPlayer;
	private bool[] playersActive;
	private bool[] playersChosen;
	private bool hasGameStarted;

	private void Awake()
	{
		gameManager = GameManager.Instance;
	}

	private void Start()
	{
		playersChosen = new bool[maxPlayersNumb];
		playersActive = new bool[maxPlayersNumb];
		hasGameStarted = false;
	}

	// Update is called once per frame
	private void Update()
	{
		if (gameManager.GameState == GameState.CharacterSelect)
		{
			SetPlayers();
			StartGame();
		}
	}

	private void SetPlayers()
	{
		for (int i = 0; i < maxPlayersNumb; i++)
		{
			int pNumb = i + 1;
			if (!playersActive[i])
			{
				if (Input.GetButtonDown($"P{pNumb} Choose"))
				{
					playersActive[i] = true;

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

	private void StartGame()
	{
		if (gameManager.Players.Count >= 2 &&
			Input.GetButton("PS Button"))
		{
			gameManager.GameState = GameState.Match;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

	public void SetChoosingPlayer(int player) => choosingPlayer = player;

	public void ChooseCharacter(int characterID)
	{
		//if (!playersChosen[choosingPlayer - 1])
		//{
		//playersChosen[choosingPlayer - 1] = true;
		gameManager.Players[choosingPlayer - 1].SetPlayerCharacter(allHeroes[characterID]);
		UpdateText(characterID);
		//}
	}

	private void UpdateText(int characterID)
	{
		string heroName = allHeroes[characterID].name;

		selectedHeroes[choosingPlayer - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
			= $"PLAYER {choosingPlayer} \n\n {heroName}";
	}
}
