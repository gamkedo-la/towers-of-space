using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    private bool musicMute = false;
    private bool sfxMute = false;

	public Toggle toggleMusic;
	public Toggle toggleSound;

	public Text d;

	void Start()
	{
		musicMute = PlayerPrefs.GetString("Mute music", "off") == "on";
		sfxMute = PlayerPrefs.GetString("Mute sfx", "off") == "on";

		d.text += "\nloaded music mute:" + System.Convert.ToSingle(musicMute);
		d.text += "\nloaded sfx mute:" + System.Convert.ToSingle(sfxMute);

		if (musicMute)
		{
			toggleMusic.isOn = false;
		}
		if (sfxMute)
		{
			toggleSound.isOn = false;
		}
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
		PlayerPrefs.SetString("Mute music", musicMute ? "on" : "off");
		d.text += "\ntoggle music mute:" + System.Convert.ToSingle(musicMute);

		#if !UNITY_EDITOR_LINUX
			d.text += " ...";
            AkSoundEngine.SetRTPCValue("Music_Mute", System.Convert.ToSingle(musicMute));  // 1 for mute, 0 for unmute
		#endif
    }

    public void ToggleSound(bool sound)
    {
		playUIButtonSound ();

		sfxMute = !sound;
		PlayerPrefs.SetString("Mute sfx", sfxMute ? "on" : "off");
		d.text += "\ntoggle sound mute:" + System.Convert.ToSingle(sfxMute);

		#if !UNITY_EDITOR_LINUX
			d.text += " ...";
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
