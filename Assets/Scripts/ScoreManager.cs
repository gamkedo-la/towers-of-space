using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public int lives = 20;
    public int energy = 20;

    public Text livesText;
    public Text energyText;

    public void LoseLife(int l = 1) {
        lives -= l;
        if (lives <= 0) {
            GameOver();
        }

        livesText.text = "Lives: "  + lives.ToString();
    }

    public bool HasEnergy(int e = 1) {
        return e <= energy;
    }

    public void UseEnergy(int e = 1) {
        energy -= e;

        energyText.text = "Energy: " + energy.ToString ();
    }

    public void GameOver() {
        Debug.Log("Game Over");
        // TODO: Send the player to a game-over screen instead!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
