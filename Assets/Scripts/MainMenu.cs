using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IOnComplete {

	public CameraMover cameraMover;
	public Canvas gameMenu;
	public GameManager gameManager;

	public void StartOnePlayer()
	{
		Config.isTwoPlayer = false;
		ShowGame();
	}

	public void StartTwoPlayer()
	{
		Config.isTwoPlayer = true;
		ShowGame();
	}

	public void ReturnToMenu()
	{
		
	}

	public void EndGame()
	{
		gameMenu.enabled = false;
		gameManager.DestroyAllBalls();
		cameraMover.MoveToMenu(this, "ShowMenu");
	}

	public void ExitGame()
	{
	#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
     #else
         Application.Quit();
     #endif
    }

	void ShowGame()
	{
		transform.GetComponent<Canvas>().enabled = false;
		cameraMover.MoveToGame(this, "StartGame");
	}

	public void OnComplete(string action)
	{
		if (action == "StartGame")
		{
			gameMenu.enabled = true;
			gameManager.StartGame();
		}
		else if (action == "ShowMenu")
		{
			transform.GetComponent<Canvas>().enabled = true;
		}
	}

	void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			ExitGame();
		}
	}
}
