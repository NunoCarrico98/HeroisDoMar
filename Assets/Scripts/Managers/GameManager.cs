using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
}
