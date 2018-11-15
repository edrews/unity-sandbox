using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartOnePlayer()
	{
		Config.isTwoPlayer = false;
		SceneManager.LoadScene(1);
	}

	public void StartTwoPlayer()
	{
		Config.isTwoPlayer = true;
		SceneManager.LoadScene(1);
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void ExitGame()
	{
	#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
     #else
         Application.Quit();
     #endif
    }
}
