using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public int PlayerNumber { get; private set; }
	public Color Color { get; private set; }
	public GameObject PlayerHero { get; private set; }

	public Player(int playerNumber, Color color)
	{
		PlayerNumber = playerNumber;
		Color = color;
	}

	public void SetPlayerCharacter(GameObject playerHero) => this.PlayerHero = playerHero;

	public void SetupPlayer(int playerNumber, GameObject Hero, Color color)
	{
		PlayerNumber = playerNumber;
		Color = color;
		PlayerHero = Hero;
	}
}
