using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    GameObject spawnNode;
    public GameObject nextNode;

    private bool hasNextWaypoint = false;
    private Vector3 nextNodeWaypoint;

    public float speed = 1f;
    public float turnRate = 5f;

    private float movementSpeed;

    private float timeToLerp;
    private float timeStartedLerping;
    private Quaternion facingBeforeLerping;

    private IEnumerator slowMovementCoroutine;

    void Awake() {
        movementSpeed = speed;
    }

    public void SlowMovement(float speedModifier, float timeToSlowDown) {
        if (slowMovementCoroutine != null) {
            StopCoroutine(slowMovementCoroutine);
        }

        slowMovementCoroutine = DoSlowMovement(speedModifier, timeToSlowDown);
        StartCoroutine(slowMovementCoroutine);
    }

    private IEnumerator DoSlowMovement(float speedModifier, float timeToSlowDown) {
        movementSpeed = speed * speedModifier;

        yield return new WaitForSeconds(timeToSlowDown);

        movementSpeed = speed;
    }

    public void SetSpawnNode(GameObject newSpawnNode) {
        spawnNode = newSpawnNode;
    }

    void SetNextNode() {
        if (spawnNode == null) {
            return;
        }

        Node n;
        if (nextNode == null) {
            n = spawnNode.GetComponent <Node> ();
        }
        else {
            n = nextNode.GetComponent <Node> ();
        }
        if (n) {
            int index = Random.Range (0, n.nextNodes.Length);
            nextNode = n.nextNodes[index];

            if (nextNode) {
                hasNextWaypoint = true;

                nextNodeWaypoint = nextNode.transform.position;
            }
        }
    }
	
	// Update is called once per frame
    void Update () {
        if (!hasNextWaypoint) {
            SetNextNode ();

            if (!hasNextWaypoint) {
                // No more path nodes!
                ReachedGoal ();
            }
        }

        Vector3 direction = nextNodeWaypoint - transform.localPosition;
        float distanceThisFrame = movementSpeed * Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame) {
            // Reached node
            hasNextWaypoint = false;
        }
        else {
            // Move towards node
            transform.Translate (direction.normalized * distanceThisFrame, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation (direction);
            transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnRate);
        }
	}

    void ReachedGoal() {
        gameObject.GetComponent <Enemy> ().ReachedGoal ();
    }
}
