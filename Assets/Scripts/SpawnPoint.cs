using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public Transform enemies;

    private Vector3 pos;
    private Quaternion lookAt;
    private Node node;

	void Start () {
        pos = transform.localPosition;

        node = GetComponent <Node> ();
        int index = Random.Range (0, node.nextNodes.Length);
        GameObject nextNode = node.nextNodes[index];
        if (nextNode) {
            Vector3 nodePosition = nextNode.transform.position;
            lookAt = Quaternion.LookRotation (nodePosition - pos);
        }
    }
	
    public void SpawnEnemy (GameObject enemy) {
        GameObject e = GameObject.Instantiate (enemy, pos, lookAt);
        e.GetComponent <EnemyMovement> ().SetSpawnNode(gameObject);
        e.transform.SetParent(enemies);
	}
}
