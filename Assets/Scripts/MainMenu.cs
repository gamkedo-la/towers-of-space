using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LoadScene(string sceneName)
	{
		ResetTimeScale();
		SceneManager.LoadScene(sceneName);
	}

	public void ReloadCurrentScene()
	{
        LoadScene(SceneManager.GetActiveScene().name);
	}

	protected void ResetTimeScale()
	{
		if (GameController.instance)
		{
			GameController.instance.isPaused = false;
			GameController.instance.ResetTimeScale();
		}
	}

}
