using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ProjectileBase : MonoBehaviour {

    public float speed = 15f;
    public float turnRate = 5f;

    protected Transform target;

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
		if ( target == null )
			Destroy( gameObject );

        if (target.gameObject == other.gameObject)
        {
            DoProjectileHit();
        }
    }

	protected abstract void DoProjectileHit();
}
