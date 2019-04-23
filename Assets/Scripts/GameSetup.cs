using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private Image[] selectedHeroes;
    [SerializeField] private int maxPlayersNumb;

    private static GameSetup instance;

    private bool[] playersChosen;
    private string[] heroNames;
    private bool hasGameStarted;

    public Dictionary<int, int> pControllerNumbers { get; private set; }
    public static GameSetup Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playersChosen = new bool[maxPlayersNumb];
        pControllerNumbers = new Dictionary<int, int>();
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
        if (playersChosen.Any(pChoice => !pChoice) && !hasGameStarted)
        {
            for (int i = 0; i < maxPlayersNumb; i++)
            {
                if (!playersChosen[i])
                {
                    if (Input.GetButtonDown($"P{i + 1} Choose"))
                    {
                        int pNumb = pControllerNumbers.Count + 1;
                        playersChosen[i] = true;
                        selectedHeroes[pNumb - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text 
                            = $"PLAYER {pNumb} SELECTED {heroNames[pNumb - 1]}";
                        pControllerNumbers.Add(pControllerNumbers.Count + 1, i + 1);
                    }
                }
            }
        }
    }

    private void StartGame()
    {
        if (pControllerNumbers.Count >= 2 && Input.GetButton("PS Button") && !hasGameStarted)
        {
            hasGameStarted = true;
            Debug.Log("Game Started!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
