using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public bool musicMute = false;
    public bool sfxMute = false;

	public Toggle toggleMusic;
	public Toggle toggleSound;

	void Start()
	{
		musicMute = PlayerPrefs.GetInt("Music", musicMute ? 1 : 0) == 1;
		sfxMute = PlayerPrefs.GetInt("Sound", sfxMute ? 1 : 0) == 1;
		toggleMusic.isOn = !musicMute;
		toggleSound.isOn = !sfxMute;
		ToggleMusic(musicMute);
		ToggleSound(sfxMute);
	}

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

    public void ToggleMusic(bool music)
    {
		playUIButtonSound ();

		musicMute = !music;
		PlayerPrefs.SetInt("Music", musicMute ? 1 : 0);
		Debug.Log("mute music?" + (musicMute ? "on":"off"));
		Debug.Log(System.Convert.ToSingle(musicMute));

		#if !UNITY_EDITOR_LINUX
            AkSoundEngine.SetRTPCValue("Music_Mute", System.Convert.ToSingle(musicMute));  // 1 for mute, 0 for unmute
		#endif
    }

    public void ToggleSound(bool sound)
    {
		playUIButtonSound ();

		sfxMute = !sound;
		PlayerPrefs.SetInt("Sound", sfxMute ? 1 : 0);
		Debug.Log("mute sound?" + (sfxMute ? "on":"off"));
		Debug.Log(System.Convert.ToSingle(sfxMute));

		#if !UNITY_EDITOR_LINUX
            AkSoundEngine.SetRTPCValue("SFX_Mute", System.Convert.ToSingle(sfxMute));  // 1 for mute, 0 for unmute
		#endif
    }

	public void playUIButtonSound() {
		#if !UNITY_EDITOR_LINUX
			AkSoundEngine.PostEvent("Play_UI_Button", this.gameObject);
		#endif

	}

	public void playUITowerSound() {
		#if !UNITY_EDITOR_LINUX
			AkSoundEngine.PostEvent("Play_UI_Tower", this.gameObject);
		#endif
	}

}
