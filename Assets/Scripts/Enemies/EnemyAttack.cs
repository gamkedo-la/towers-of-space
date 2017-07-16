using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrel;
    public float rotationSpeed = 60f;
    public Transform[] muzzles;
    public float range = 6f;
    public float fireCooldown = 1f;

    float fireCooldownLeft = 0f;
    int muzzleIndex = 0;

    private static Transform projectileGroup;

	public GameObject muzzleflashPrefab;

    private Tower targetedTower;
    private IEnumerator rotateBarrelForwardCorouting;

	// Use this for initialization
    void Start()
    {
        if (projectileGroup == null)
        {
            projectileGroup = GameObject.Find("Projectiles").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Tower newTarget = FindTowerTarget();

        if (newTarget == null)
        {
            // Only rotate back when we were locked onto an enemy
            if (targetedTower != null)
            {
                targetedTower = null;
                rotateBarrelForwardCorouting = RotateBarrelForward();
                StartCoroutine(rotateBarrelForwardCorouting);
            }

            return;
        }
        else if (rotateBarrelForwardCorouting != null)
        {
            StopCoroutine(rotateBarrelForwardCorouting);
            rotateBarrelForwardCorouting = null;
        }

        targetedTower = newTarget;

        Vector3 direction = targetedTower.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(direction);
        barrel.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        fireCooldownLeft -= Time.deltaTime;
        if (fireCooldownLeft <= 0 && direction.magnitude <= range)
        {
            fireCooldownLeft = fireCooldown;
            ShootAt(targetedTower);
        }
    }

    Tower FindTowerTarget()
    {
        // If the current target is still in range and not destroyed, keep targetting that.
        if (targetedTower != null && !targetedTower.IsDestroyed())
        {
            float d = Vector3.Distance(transform.position, targetedTower.transform.position);
            if (d <= range)
            {
                return targetedTower;
            }
        }

        Tower[] towers = GameObject.FindObjectsOfType<Tower>();

        Tower nearestTower = null;

        float dist = Mathf.Infinity;

        foreach (Tower tower in towers)
        {
            if (tower.IsDestroyed())
            {
                continue;
            }

            float d = Vector3.Distance(transform.position, tower.transform.position);
            if (d <= range && (nearestTower == null || d < dist))
            {
                nearestTower = tower;
                dist = d;
            }
        }

        return nearestTower;
    }

    private IEnumerator RotateBarrelForward()
    {
        yield return new WaitForSeconds(1f);

        bool done = false;
        float maxDegrees = Time.deltaTime * rotationSpeed;
        while (!done)
        {
            barrel.rotation = Quaternion.RotateTowards(barrel.rotation, transform.rotation, maxDegrees);
            done = (Mathf.Abs(barrel.rotation.eulerAngles.y - transform.rotation.eulerAngles.y) <= maxDegrees);

            yield return new WaitForEndOfFrame ();
        }

        barrel.rotation = transform.rotation;
    }

    void ShootAt(Tower enemy)
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, muzzles[muzzleIndex].position, muzzles[muzzleIndex].rotation);
        bulletGO.transform.SetParent(projectileGroup);
        bulletGO.GetComponent<ProjectileBase>().SetTarget(enemy.transform);

		if (muzzleflashPrefab)
		{
			GameObject muzzy = Instantiate(muzzleflashPrefab, muzzles[muzzleIndex].position, muzzles[muzzleIndex].rotation);
			Destroy(muzzy,3); // FIXME: reuse pool
		}

		muzzleIndex++;
        if (muzzles.Length <= muzzleIndex)
        {
            muzzleIndex = 0;
        }
    }
}
