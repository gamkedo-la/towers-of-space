using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {
	public float timeBeforeWave = 10f; //Time before first wave. Will be overwritten with time to beat wave
	public float timeLeft = 10f; 
	SpawnPoint[] spawnPoints;
	// Use this for initialization
	void Start () {
		spawnPoints = FindObjectsOfType<SpawnPoint>( );
		//queueNextWave ();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			queueNextWave ();
		}
		UIController.instance.UpdateSpawnBar (timeLeft / timeBeforeWave);
	}

	void queueNextWave () {
		timeBeforeWave = 0f;
		string msg = "Queing next wave \n";
		foreach (SpawnPoint spawnPoint in spawnPoints) {			
			msg += "Spawn Point: " + spawnPoint.name + "\n";
			EnemySpawner[] spawners = spawnPoint.Waves.GetComponentsInChildren<EnemySpawner>(true);

			foreach (EnemySpawner spawner in spawners) {
				if (!spawner.gameObject.activeSelf) {
					
					if(timeBeforeWave < spawner.timeBeforeSpawning){
						timeBeforeWave = spawner.timeBeforeSpawning;
						timeLeft = spawner.timeBeforeSpawning;
					}
					msg += "  Activating Wave: " + spawner.name + "\n";
					spawner.gameObject.SetActive (true);
					GameController.instance.RefoundDeconstructs( );
					break;
				}
			}
		}
		Debug.Log(msg);
	}
}
