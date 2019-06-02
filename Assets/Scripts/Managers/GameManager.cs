using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private CameraSingleton cam;

	public GameState GameState { get; set; }

	public static GameManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		else Instance = this;

		DontDestroyOnLoad(gameObject);

		cam = FindObjectOfType<CameraSingleton>();
	}

	private void Start()
	{
		GameState = GameState.MainMenu;
	}

	public void ActivateCameraController(bool active)
	{
		cam.GetComponent<CameraController>().enabled = active;
	}
}
