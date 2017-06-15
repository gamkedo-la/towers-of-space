using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : ProjectileBase
{

    public float speedModifier = 0.25f;
    public float timeToSlowDown = 3f;

    protected override void DoProjectileHit()
    {
        // todo spawn particles
        target.GetComponent<EnemyMovement>().SlowMovement(speedModifier, timeToSlowDown);

		ExplodeFX(target.transform.position,target.transform.rotation); 

        Destroy (gameObject);
    }

}
