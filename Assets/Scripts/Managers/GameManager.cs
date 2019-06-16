using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("Player")]
	[SerializeField] private Player playerPrefab;

	[Header("Positions")]
	[SerializeField] private Vector3[] initialPositions;

	[Header("Rotations")]
	[SerializeField] private Quaternion[] initialRotations;

	private CameraController cam;
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
		GameManagerSingleton();

		cam = FindObjectOfType<CameraController>();
		Players = new List<Player>(4);
	}

	private void GameManagerSingleton()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		else Instance = this;

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		GameState = GameState.MainMenu;
	}

	private void SetupMatch()
	{
		uiManager = FindObjectOfType<UIManager>();

		for (int i = 0; i < Players.Count; i++)
		{
			Player p = Instantiate(playerPrefab, initialPositions[i], initialRotations[i]);
			p.name = $"Player {i + 1}";
			GameObject hero = Instantiate(Players[i].PlayerHero, p.transform);

			p.SetupPlayer(Players[i].PlayerNumber, Players[i].PlayerHero, Players[i].Color);
			hero.GetComponent<Hero>().SetupHero(p.PlayerNumber, p.Color);
		}
	}

	public void OnLevelLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex == 2)
        {
            SetupMatch();
			cam.SetupCamera();
			ActivateCameraController(true);
			StartCoroutine(SoundManager.Instance.MusicFadeOut());
			Players = FindObjectsOfType<Player>().ToList();
        }
	}

	public void ReloadMatch()
	{
		Players = new List<Player>(4);
		ActivateCameraController(false);
		GameState = GameState.Match;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		SetupMatch();
		cam.SetupCamera();
		ActivateCameraController(true);
		Players = FindObjectsOfType<Player>().ToList();
	}

	public void ActivateCameraController(bool flag) => cam.enabled = flag;

	public void CreateNewPlayer(int i, Color color) => Players.Add(new Player(i, color));
}
