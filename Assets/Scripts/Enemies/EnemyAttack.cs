using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrel;
    public Transform[] muzzles;
    public float range = 6f;
    public float fireCooldown = 1f;

    float fireCooldownLeft = 0f;
    int muzzleIndex = 0;

    private static Transform projectileGroup;

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
        Tower[] enemies = GameObject.FindObjectsOfType<Tower>();

        Tower nearestEnemy = null;

        float dist = Mathf.Infinity;

        foreach (Tower enemy in enemies)
        {
            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (nearestEnemy == null || d < dist)
            {
                nearestEnemy = enemy;
                dist = d;
            }
        }

        if (nearestEnemy == null)
        {
            return;
        }

        Vector3 direction = nearestEnemy.transform.position - this.transform.position;

        Quaternion rotation = Quaternion.LookRotation(direction);
        barrel.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        fireCooldownLeft -= Time.deltaTime;
        if (fireCooldownLeft <= 0 && direction.magnitude <= range)
        {
            fireCooldownLeft = fireCooldown;
            ShootAt(nearestEnemy);
        }
    }

    void ShootAt(Tower enemy)
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, muzzles[muzzleIndex].position, muzzles[muzzleIndex].rotation);
        bulletGO.transform.SetParent(projectileGroup);

        bulletGO.GetComponent<ProjectileBase>().SetTarget(enemy.transform);

        muzzleIndex++;
        if (muzzles.Length <= muzzleIndex)
        {
            muzzleIndex = 0;
        }
    }
}
