using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameState GameState { get; set; }

	public static GameManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		else Instance = this;

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		GameState = GameState.MainMenu;
	}

	// Update is called once per frame
	private void Update()
    {
		ResetGame();
    }

	private void ResetGame()
	{
		if (Input.GetKeyDown(KeyCode.Joystick1Button9))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
