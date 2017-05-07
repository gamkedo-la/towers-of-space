using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public Transform enemies;

    private Vector3 pos;
    private Quaternion lookAt;
    private Node node;

	void Start () {
        pos = transform.position;
        //pos.y += 1;

        node = GetComponent <Node> ();

        // @todo fix look rotation, first thing an enemy does, is rotate around?!

        Vector3 nodePosition = node.transform.position;
        //nodePosition.y += 1;
        lookAt = Quaternion.LookRotation (nodePosition - pos);
    }
	
    public void SpawnEnemy (GameObject enemy) {
        GameObject e = GameObject.Instantiate (enemy, pos, lookAt);
        e.GetComponent <EnemyMovement> ().SetSpawnNode(gameObject);
	}
}
