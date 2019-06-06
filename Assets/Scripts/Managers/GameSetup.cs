using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
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

    private static GameSetup instance;
	private GameManager gameManager;

    private int choosingPlayer;

    private bool[] playersActive;
    private bool[] playersChosen;
    //private string[] heroNames;
    private bool hasGameStarted;

    //public Dictionary<int, Tuple<int, Characters>> pControllerNumbers { get; private set; }
    public static GameSetup Instance => instance;

    private void Awake()
    {
		gameManager = GameManager.Instance;
    }

    private void Start()
    {
        playersChosen = new bool[maxPlayersNumb];
        playersActive = new bool[maxPlayersNumb];
        //pControllerNumbers = new Dictionary<int, Tuple<int, Characters>>();
        hasGameStarted = false;
        //heroNames = new string[2] { "Fernando Pessoa", "Padeira de Aljubarrota" };
    }
    // Update is called once per frame
    private void Update()
    {
        ChooseCharacters();
        StartGame();
    }

    private void ChooseCharacters()
    {
        if (gameManager.GameState == GameState.CharacterSelect)
        {
            for (int i = 0; i < maxPlayersNumb; i++)
            {
                if (!playersActive[i])
                {
                    if (Input.GetButtonDown($"P{i + 1} Choose"))
                    {
                        int pNumb = gameManager.Players.Count + 1;

                        //pControllerNumbers.Add(pNumb, new Tuple<int, Characters>(i + 1, 0));
						//gameManager.Players.Add(new Player(pNumb, playerColors[i]));
						gameManager.CreateNewPlayer(pNumb, playerColors[i]);

						Debug.Log($"{i} {pNumb}");
                        playersActive[i] = true;

                        StandaloneInputModule inputModule = playersES[pNumb - 1].GetComponent<StandaloneInputModule>();

                        inputModule.horizontalAxis = $"P{i + 1} Menu Control Horizontal";
                        inputModule.verticalAxis = $"P{i + 1} Menu Control Vertical";
                        inputModule.submitButton = $"P{i + 1} Choose";
                        inputModule.cancelButton = $"P{i + 1} Cancel";

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
                }
            }
        }
    }

    private void StartGame()
    {
        if (gameManager.Players.Count >= 2 && 
			Input.GetButton("PS Button") && 
			gameManager.GameState == GameState.CharacterSelect)
        {
			gameManager.GameState = GameState.Match;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void SetChoosingPlayer(int player) => choosingPlayer = player;

    public void ChooseCharacter(int characterID)
    {
		//int joystickNum = pControllerNumbers[choosingPlayer].Item1;
		//pControllerNumbers[choosingPlayer] = new Tuple<int, Characters>(joystickNum, (Characters)characterID);

		//Debug.Log($"Player {choosingPlayer} with joystick {pControllerNumbers[choosingPlayer].Item1} has chosen " +
		//	$"{pControllerNumbers[choosingPlayer].Item2}");

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
		//Characters characterChosen = pControllerNumbers[choosingPlayer].Item2;

  //      switch (characterChosen)
  //      {
  //          case Characters.FernandoPessoa:
  //              heroName = heroNames[0];
  //              break;
  //          case Characters.PadeiraDeAljubarrota:
  //              heroName = heroNames[1];
  //              break;
  //      }

        selectedHeroes[choosingPlayer - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text 
            = $"PLAYER {choosingPlayer} \n\n {heroName}";
    }
}
