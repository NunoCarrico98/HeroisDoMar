using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("Player")]
	[SerializeField] private Player playerPrefab;

	[Header("Positions")]
	[SerializeField] private Vector3[] initialPositions;

	[Header("Rotations")]
	[SerializeField] private Quaternion[] initialRotations;

	private CameraSingleton cam;
	private UIManager uiManager;

	public GameState GameState { get; set; }
	public List<Player> Players { get; private set; }

	public static GameManager Instance { get; private set; }

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelLoaded;
	}

	private void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		else Instance = this;

		DontDestroyOnLoad(gameObject);

		cam = FindObjectOfType<CameraSingleton>();
		Players = new List<Player>(4);
	}

	private void Start()
	{
		GameState = GameState.MainMenu;
	}

	private void SetupMatch()
	{
		if (GameState == GameState.Match)
		{
			uiManager = FindObjectOfType<UIManager>();

			for (int i = 0; i < Players.Count; i++)
			{
				Player p = Instantiate(playerPrefab, initialPositions[i], initialRotations[i]);
				p.name = $"Player {i + 1}";

				GameObject hero = Instantiate(Players[i].PlayerHero, p.transform);

				p.SetupPlayer(Players[i].PlayerNumber, Players[i].PlayerHero, Players[i].Color);
				hero.GetComponent<Hero>().SetupHero(p.PlayerNumber);
			}
		}
	}

	public void OnLevelLoaded(Scene scene, LoadSceneMode mode)
	{
		SetupMatch();
	}

	public void ActivateCameraController(bool active) => cam.GetComponent<CameraController>().enabled = active;

	public void CreateNewPlayer(int i, Color color) => Players.Add(new Player(i, color));
}
