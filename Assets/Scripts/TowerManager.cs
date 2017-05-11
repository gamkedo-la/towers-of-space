﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public GameObject selectedTowerType;

    public GameObject[] TowerTypes;
    public bool isPaused = false;
    public GameObject pausePanel;
    public float gameTimeScale = 1.0f;

    public void SelectTowerType(GameObject prefab)
    {
        selectedTowerType = prefab;
    }

    public void ChangeTowerType(int towerTypeIndex)
    {
        if (isPaused == false)
        {
            selectedTowerType = TowerTypes[towerTypeIndex];
        }
    }

    public void TogglePause()
    {
        if (isPaused == true)
        {
            isPaused = false;
            Time.timeScale = gameTimeScale;
            pausePanel.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }

    public void SlowTime()
    {
        if (isPaused == false)
        {
            gameTimeScale = Mathf.Max(gameTimeScale / 2, 0.25f);
            Time.timeScale = gameTimeScale;
        }
    }

    public void ResetTimeScale()
    {
        if (isPaused == false)
        {
            gameTimeScale = 1.0f;
            Time.timeScale = gameTimeScale;
        }
    }

    public void SpeedUpTime()
    {
        if (isPaused == false)
        {
            gameTimeScale = Mathf.Min(gameTimeScale * 2, 4);
            Time.timeScale = gameTimeScale;
        }
    }
}

