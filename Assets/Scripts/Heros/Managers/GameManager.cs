using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
