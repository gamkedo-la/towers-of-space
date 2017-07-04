﻿using System.Collections;
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
		SceneManager.LoadScene(sceneName);
	}

	public void ReloadCurrentScene()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
