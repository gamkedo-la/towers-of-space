using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AltSpawnPoint : MonoBehaviour
{
	public GameObject Waves;

    private Vector3 pos;
    private Quaternion lookAt;
    private Node node;
    private static Transform enemyGroup;

    void Start()
    {
		Assert.IsNotNull( Waves );

        pos = transform.position;

        node = GetComponent<Node>();
        int index = Random.Range(0, node.nextNodes.Length);
        GameObject nextNode = node.nextNodes[index];
        if (nextNode)
        {
            Vector3 nodePosition = nextNode.transform.position;
            lookAt = Quaternion.LookRotation(nodePosition - pos);
        }
    }

    public void SpawnEnemy(GameObject enemy)
    {
        GameObject e = GameObject.Instantiate(enemy, pos, lookAt);
        e.GetComponent<EnemyMovement>().SetSpawnNode(gameObject);
        if (enemyGroup == null)
        {
            enemyGroup = GameObject.Find("Enemies").transform;
        }
        e.transform.SetParent(enemyGroup);
    }
}
