using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
	[Header("Winner")]
	[SerializeField] private TextMeshProUGUI winnerText;
	[SerializeField] private Image winnerImage;

	[Header("Buttons")]
	[SerializeField] private Button rematchButton;
	[SerializeField] private Button mainMenuButton;

	[Header("E")]

	private List<Hero> players;
	private Animator anim;
	private GameManager gameManager;
	private StandaloneInputModule inputModule;

	private void Start()
	{
		anim = GetComponent<Animator>();
		players = FindObjectsOfType<Hero>().ToList();
		gameManager = GameManager.Instance;
		inputModule = FindObjectOfType<StandaloneInputModule>();
	}

	// Update is called once per frame
	void Update()
	{
		if(gameManager.GameState != GameState.VictoryScreen) CheckWin();
	}

	private void CheckWin()
	{
		for (int i = 0; i < players.Count; i++)
		{
			if (players[i].Dead) players.RemoveAt(i);
			if (players.Count == 1) StartCoroutine(ShowWinScreen(players[0]));
		}
	}

	private IEnumerator ShowWinScreen(Hero p)
	{
		yield return new WaitForSeconds(0.1f);

		gameManager.GameState = GameState.VictoryScreen;
		winnerText.text = $"Player {p.PlayerNumber}";
		winnerImage.sprite = p.VictoryScreenImage;
		anim.SetTrigger("Win");

		inputModule.horizontalAxis = $"P{p.PlayerNumber} Menu Control Horizontal";
		inputModule.verticalAxis = $"P{p.PlayerNumber} Menu Control Vertical";
		inputModule.submitButton = $"P{p.PlayerNumber} Choose";
		inputModule.cancelButton = $"P{p.PlayerNumber} Cancel";
		rematchButton.Select();
		rematchButton.OnSelect(null);
	}

	public void ClickRematch()
	{
		gameManager.ReloadMatch();
	}

	public void ClickMainMenu()
	{
		gameManager.GameState = GameState.MainMenu;
		SceneManager.LoadScene("MainMenu");
	}
}
