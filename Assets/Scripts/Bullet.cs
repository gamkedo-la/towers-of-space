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

        // Move towards enemy
        Vector3 direction = target.position - transform.localPosition;
        transform.Translate (direction.normalized * speed * Time.deltaTime, Space.World);
        Quaternion targetRotation = Quaternion.LookRotation (direction);
        transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnRate);
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(Tags.Enemy)) {
            DoBulletHit();
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
