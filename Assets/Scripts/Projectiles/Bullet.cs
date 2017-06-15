using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ProjectileBase
{

    public float damage = 0.5f;
    public float radius = 0;

    protected override void DoProjectileHit()
    {
        if (radius == 0)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
				ExplodeFX(enemy.transform.position,enemy.transform.rotation); 
            }
            Tower tower = target.GetComponent<Tower>();
            if (tower != null)
            {
                tower.TakeDamage(damage);
				ExplodeFX(tower.transform.position,tower.transform.rotation); 
            }
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider collider in colliders)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
					ExplodeFX(enemy.transform.position,enemy.transform.rotation); 
                }
                Tower tower = collider.GetComponent<Tower>();
                if (tower != null)
                {
                    enemy.GetComponent<Tower>().TakeDamage(damage);
					ExplodeFX(tower.transform.position,tower.transform.rotation); 
                }
            }
        }

        Destroy (gameObject);
    }
}
