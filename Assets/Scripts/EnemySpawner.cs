using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    
    public float timeBeforeSpawning = 4f;
    public float timeBetweenEnemies = 0.25f;

    [System.Serializable]
    public class WaveComponent {
        public GameObject enemyPrefab;
        public int num;
        [System.NonSerialized]
        public int spawned = 0;
    }

    public WaveComponent[] waveComponents;

    float spawnCDremaining = .5f;

    void OnEnable() {
        spawnCDremaining = timeBeforeSpawning;
    }

    void Update () {
        spawnCDremaining -= Time.deltaTime;
        if (spawnCDremaining < 0) {
            spawnCDremaining = timeBetweenEnemies;

            bool didSpawn = false;

            // Go through the wave components until we find something to spawn
            foreach (WaveComponent wc in waveComponents) {
                if (wc.spawned < wc.num) {
                    // Spawn it!
                    wc.spawned++;
                    transform.parent.parent.GetComponent <SpawnPoint> ().SpawnEnemy (wc.enemyPrefab);

                    didSpawn = true;
                    // Stop the loop because we need to wait before spawning the next enemy
                    break;
                }
            }

            // If nothing has spawned, it means all waves components are done, so activate next wave!
            if (!didSpawn) {
                if (transform.parent.childCount > 1) {
                    transform.parent.GetChild(1).gameObject.SetActive(true);
                }

                Destroy(gameObject);
            }
        }
    }
}
