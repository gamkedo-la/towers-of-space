using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsControl : MonoBehaviour
{
	private Button button;

	void Start ()
	{
		button = GetComponent<Button>( );

		if (SceneManager.sceneCountInBuildSettings - 1 <= SceneManager.GetActiveScene().buildIndex)
		{
			Debug.Log( SceneManager.sceneCountInBuildSettings );
			if (button != null) button.interactable = false;
		}
	}
	
	public void NextLevel()
	{
		if ( SceneManager.sceneCountInBuildSettings - 1 <= SceneManager.GetActiveScene( ).buildIndex )
			return;

		if ( GameController.instance )
		{
			GameController.instance.isPaused = false;
			GameController.instance.ResetTimeScale( );
		}

		SceneManager.LoadScene( SceneManager.GetActiveScene( ).buildIndex + 1 );
	}

	public void ReloadLevel( )
	{
		SceneManager.LoadScene( SceneManager.GetActiveScene( ).buildIndex );
	}
}
