using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float health = 8f;
    float startHealth;
    Slider slider;

    public GameObject bulletPrefab;
    public Transform barrel;
    public Transform[] muzzles;
    public float range = 6f;
    public float fireCooldown = 1f;
    public float buildTime = 2f;
    public float newAnimationScanWidth = 0.1f;
    private float towerHeight, scanStartY;
    private bool building;
    const float platformHeight = -0.5f;
    private Material[] materials;
    public GameObject selectionCirclePrefab;
    private GameObject selectionCircle;

    public int[] costLadder; //Sequence of "ramping up" energy costs

    float fireCooldownLeft = 0f;
    float buildTimeLeft;
    int muzzleIndex = 0;

    private static Transform projectileGroup;

    TowerSpot parentSpot; //Self-explanatory

    // Use this for initialization

    void Start()
    {
        if (projectileGroup == null)
        {
            projectileGroup = GameObject.Find("Projectiles").transform;
        }
        slider = transform.FindChild("TowerCanvas").FindChild("Slider").GetComponent<Slider>();
        startHealth = health;

        buildTimeLeft = buildTime;
        building = true;
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
        foreach (Material material in materials)
        {
            material.SetFloat("_ConstructY", -1);
            material.SetFloat("_ConstructGap", newAnimationScanWidth);
        }
        //PrefabUtility.SetPropertyModifications(bulletPrefab, currentCostIndex);

        //Next two lines are a hack, will need fixing
        towerHeight = 1.1f; //total height of tower
        scanStartY = platformHeight - newAnimationScanWidth; //lowest tower y - scan width

        parentSpot = GetComponentInParent<TowerSpot>();

        StartCoroutine("BuildTower");

        selectionCircle = Instantiate(selectionCirclePrefab);
        selectionCircle.transform.SetParent(transform, false);
        selectionCircle.SetActive(false);
        Projector p = selectionCircle.GetComponent<Projector>();
        // Why 0.6f? No clue.. seemed to approximate the line radius
        p.orthographicSize = range + 0.6f;
    }

    private IEnumerator BuildTower()
    {
        LaserBuilderEffect laserEffect = parentSpot.GetComponent<LaserBuilderEffect>();
        laserEffect.beginConstruction();

        while (buildTimeLeft > 0)
        {
            buildTimeLeft -= Time.deltaTime;
            float t = buildTimeLeft / buildTime;

            if (buildTimeLeft < 0)
            {
                building = false;
                t = 0;
            }

            foreach (Material material in materials)
            {
                material.SetFloat("_ConstructY", scanStartY - (t * towerHeight - towerHeight));
            }

            laserEffect.setHeight(1 - t);
            yield return null;
        }

        laserEffect.endConstruction(true);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        slider.value = health / startHealth;

        if (health <= 0)
        {
            // @todo convert to rubble
            Die();
        }
    }

    public void Die()
    {
        // @todo particles/explosion
        Destroy(gameObject);
    }

    void OnDestroy() {
        parentSpot.DestroyTower();

        // Animate/place laser emitter
        if (building) {
            parentSpot.GetComponent<LaserBuilderEffect>().endConstruction(false);
        }
        else {
            parentSpot.GetComponent<LaserBuilderEffect>().reset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (building)
        {
            // Don't do anything while still building.
            return;
        }

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        Enemy nearestEnemy = null;

        float dist = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
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

    void ShootAt(Enemy enemy)
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, muzzles[muzzleIndex].position, muzzles[muzzleIndex].rotation);

        bulletGO.transform.SetParent(projectileGroup);

        bulletGO.GetComponent<ProjectileBase>().SetTarget(enemy.transform);

        muzzleIndex++;
        if (muzzles.Length <= muzzleIndex)
        {
            muzzleIndex = 0;
        }
		AkSoundEngine.PostEvent ("Play_singleTower_laser", gameObject);
    }

    public void Selected()
    {
        selectionCircle.SetActive(true);
    }

    public void Deselected()
    {
        selectionCircle.SetActive(false);
    }

    void OnMouseUp()
    {
        parentSpot.ClickedSpot();
    }
}
