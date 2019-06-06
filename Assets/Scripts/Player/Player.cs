using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	// Player Stats
	private float playerHealth;
	private float playerShield;

	public int PlayerNumber { get; private set; }
	public Color Color { get; private set; }
	public GameObject PlayerHero { get; private set; }

	public Player(int playerNumber, Color color)
	{
		this.PlayerNumber = playerNumber;
		this.Color = color;
	}

	public void SetPlayerCharacter(GameObject playerHero) => this.PlayerHero = playerHero;

	public void SetupPlayer(int playerNumber, GameObject Hero, Color color)
	{
		this.PlayerNumber = playerNumber;
		this.Color = color;
		this.PlayerHero = Hero;
		playerHealth = PlayerHero.GetComponent<Hero>().MaxHealth;
		playerShield = PlayerHero.GetComponent<Hero>().MaxShield;
	}
}
