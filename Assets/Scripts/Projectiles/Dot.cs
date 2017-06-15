using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : ProjectileBase
{
    public int numberOfTicks = 6;
    public float damagePerTick = 0.5f;
    public float timePerTick = 0.3f;

    private Enemy enemy;

    protected override void DoProjectileHit()
    {
        enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            StartCoroutine("DoDamageTicks");
        }
    }

    private IEnumerator DoDamageTicks()
    {
        float totalTime = timePerTick * numberOfTicks;
        while (0 <= totalTime)
        {
            if (enemy == null)
            {
                break;
            }

			ExplodeFX(enemy.transform.position,enemy.transform.rotation); 

			enemy.TakeDamage(damagePerTick);
            totalTime -= timePerTick;

            yield return new WaitForSeconds(timePerTick);
        }

        Destroy (gameObject);
    }
}
