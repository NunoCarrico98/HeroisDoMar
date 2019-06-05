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
    [SerializeField] private Image[] selectedHeroes;
    [SerializeField] private int maxPlayersNumb;

    [SerializeField] private Button initialP1Button;
    [SerializeField] private Button initialP2Button;

    [SerializeField] private EventSystem[] playersES;

    private static GameSetup instance;
	private GameManager gameManager;

    private int choosingPlayer;

    private bool[] playersChosen;
    private string[] heroNames;
    private bool hasGameStarted;

    public Dictionary<int, Tuple<int, Characters>> pControllerNumbers { get; private set; }
    //public Dictionary<int, int> pControllerNumbers { get; private set; }
    public static GameSetup Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);

		gameManager = GameManager.Instance;
    }

    private void Start()
    {
        playersChosen = new bool[maxPlayersNumb];
        //pControllerNumbers = new Dictionary<int, int>();
        pControllerNumbers = new Dictionary<int, Tuple<int, Characters>>();
        hasGameStarted = false;
        heroNames = new string[2] { "Fernando Pessoa", "Padeira de Aljubarrota" };
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
                if (!playersChosen[i])
                {
                    if (Input.GetButtonDown($"P{i + 1} Choose"))
                    {
                        int pNumb = pControllerNumbers.Count + 1;

                        //pControllerNumbers.Add(pNumb, i + 1);
                        pControllerNumbers.Add(pNumb, new Tuple<int, Characters>(i + 1, 0));

                        Debug.Log($"{i} {pNumb}");
                        playersChosen[i] = true;

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
        if (pControllerNumbers.Count >= 2 && 
			Input.GetButton("PS Button") && 
			gameManager.GameState == GameState.CharacterSelect)
        {
			gameManager.GameState = GameState.Match;
			Debug.Log("Game Started!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void SetChoosingPlayer(int player)
    {
        choosingPlayer = player;
    }

    public void ChooseCharacter(int characterID)
    {
        int joystickNum = pControllerNumbers[choosingPlayer].Item1;
        pControllerNumbers[choosingPlayer] = new Tuple<int, Characters>(joystickNum, (Characters) characterID);

        Debug.Log($"Player {choosingPlayer} with joystick {pControllerNumbers[choosingPlayer].Item1} has chosen " +
            $"{pControllerNumbers[choosingPlayer].Item2}");

        UpdateText();
    }

    private void UpdateText()
    {
        string heroName = null;
        Characters characterChosen = pControllerNumbers[choosingPlayer].Item2;

        switch (characterChosen)
        {
            case Characters.FernandoPessoa:
                heroName = heroNames[0];
                break;
            case Characters.PadeiraDeAljubarrota:
                heroName = heroNames[1];
                break;
        }

        selectedHeroes[choosingPlayer - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text 
            = $"PLAYER {choosingPlayer} \n\n {heroName}";
    }
}
