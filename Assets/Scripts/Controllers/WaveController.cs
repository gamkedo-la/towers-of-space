using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WaveController : MonoBehaviour {
	public float timeBeforeWave = 10f; //Time before first wave. Will be overwritten with time to beat wave
	public float timeLeft = 10f; 
	SpawnPoint[] spawnPoints;

	[SerializeField] private NextWaveDisplayer nextWave = null;

	// Use this for initialization
	void Start () {
		Assert.IsNotNull( nextWave );
		spawnPoints = FindObjectsOfType<SpawnPoint>( );
		queueFirstWave ();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			queueNextWave ();
		}
		UIController.instance.UpdateSpawnBar (timeLeft / timeBeforeWave);
	}
	void queueFirstWave(){
		UIController.instance.nextWaves.Clear();

		foreach (SpawnPoint spawnPoint in spawnPoints) {
			EnemySpawner[] spawners = spawnPoint.Waves.GetComponentsInChildren<EnemySpawner>(true);
			foreach (EnemySpawner spawner in spawners) {
				Debug.Log ("loading next wave into UI");
				GetNextWaveData( spawner );
				break;
			}
		}

		nextWave.DisplayNextWave( );
	}

	void queueNextWave () {
		timeBeforeWave = 0f;
		string msg = "Queing next wave \n";
		bool isSpawned;
		UIController.instance.nextWaves.Clear();
		foreach (SpawnPoint spawnPoint in spawnPoints) {			
			msg += "Spawn Point: " + spawnPoint.name + "\n";
			EnemySpawner[] spawners = spawnPoint.Waves.GetComponentsInChildren<EnemySpawner>(true);
			isSpawned = false;
			foreach (EnemySpawner spawner in spawners) {
				if (!spawner.gameObject.activeSelf && !isSpawned) {
					
					if(timeBeforeWave < spawner.timeBeforeSpawning){
						timeBeforeWave = spawner.timeBeforeSpawning;
						timeLeft = spawner.timeBeforeSpawning;
					}
					msg += "  Activating Wave: " + spawner.name + "\n";
					spawner.gameObject.SetActive (true);
					GameController.instance.RefoundDeconstructs( );
					isSpawned = true;
				} else
				{
					Debug.Log( "loading next wave into UI" );
					GetNextWaveData( spawner );
					break;
				}
			}
		}
		nextWave.DisplayNextWave( );
		Debug.Log(msg);
	}

	private static void GetNextWaveData( EnemySpawner spawner )
	{
		UIController.instance.nextWaves.Add( new List<EnemySpawner.WaveComponent>( ) );
		foreach ( EnemySpawner.WaveComponent wave in spawner.waveComponents )
		{
			UIController.instance.nextWaves[UIController.instance.nextWaves.Count - 1].Add( wave );
		}
	}
}
