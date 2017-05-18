using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    
    public GameObject bulletPrefab;
    public Transform barrel;
    public Transform[] muzzles;
    public float range = 6f;
    public float fireCooldown = 1f;
    public float buildTime = 2f;

    public int energy = 1;

    float fireCooldownLeft = 0f;
    float buildTimeLeft;
    int muzzleIndex = 0;

    private static Transform projectileGroup; 

	// Use this for initialization
	void Start () {
        buildTimeLeft = buildTime;
	}
	
	// Update is called once per frame
	void Update () {
        buildTimeLeft -= Time.deltaTime;
        if (0 < buildTimeLeft) {
            float t = buildTimeLeft / buildTime;

            Vector3 scale = transform.localScale;
            scale.y = Mathf.Lerp (1, 0, t);
            transform.localScale = scale;

            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp (0, -0.5f, t);
            transform.position = pos;
            return;
        }

        Enemy[] enemies = GameObject.FindObjectsOfType <Enemy> ();

        Enemy nearestEnemy = null;

        float dist = Mathf.Infinity;

        foreach (Enemy enemy in enemies) {
            float d = Vector3.Distance (transform.position, enemy.transform.position);
            if (nearestEnemy == null || d < dist) {
                nearestEnemy = enemy;
                dist = d;
            }
        }

        if (nearestEnemy == null) {
            return;
        }

        Vector3 direction = nearestEnemy.transform.position - this.transform.position;

        Quaternion rotation = Quaternion.LookRotation (direction);
        barrel.rotation = Quaternion.Euler (0, rotation.eulerAngles.y, 0);

        fireCooldownLeft -= Time.deltaTime;
        if (fireCooldownLeft <= 0 && direction.magnitude <= range) {
            fireCooldownLeft = fireCooldown;
            ShootAt (nearestEnemy);
        }
	}

    void ShootAt(Enemy enemy) {
        GameObject bulletGO = (GameObject)Instantiate (bulletPrefab, muzzles[muzzleIndex].position, muzzles[muzzleIndex].rotation);

        if (projectileGroup == null)
        {
            projectileGroup = GameObject.Find("Projectiles").transform;
        }
        bulletGO.transform.SetParent(projectileGroup);

        bulletGO.GetComponent <Bullet>().SetTarget(enemy.transform);

        muzzleIndex++;
        if (muzzles.Length <= muzzleIndex) {
            muzzleIndex = 0;
        }
    }
}
