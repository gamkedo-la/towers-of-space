using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 15f;
    public float turnRate = 5f;

    public float damage = 0.5f;
    public float radius = 0;

    Transform target;

    public void SetTarget(Transform t) {
        target = t;
    }
	
	// Update is called once per frame
	void Update () {
        if (target == null) {
            Destroy (gameObject);
            return;
        }
        Vector3 direction = target.position - transform.localPosition;
        float distanceThisFrame = speed * Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame) {
            // Reached node
            DoBulletHit ();
        }
        else {
            // Move towards node
            transform.Translate (direction.normalized * distanceThisFrame, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation (direction);
            transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnRate);
        }
	}

    void DoBulletHit() {
        if (radius == 0) {
            target.GetComponent <Enemy> ().TakeDamage (damage);
        }
        else {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider collider in colliders) {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
        }

        Destroy (gameObject);
    }
}
