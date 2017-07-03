using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float health = 8f;
    float startHealth;
    Slider slider;

    public GameObject towerGO;
    public GameObject rubbleGO;

    public GameObject bulletPrefab;
	public GameObject explosionPrefab;
	public GameObject muzzleflashPrefab;

    public Transform barrel;
    public Transform[] muzzles;
    public float range = 6f;
    public float fireCooldown = 1f;
    public float buildTime = 2f;
    public float scanWidth = 0.1f;
    private float towerHeight, scanStartY;
    private bool building;
    const float platformHeight = -0.5f;
    private Material[] materials;
    public GameObject selectionCirclePrefab;
    private GameObject selectionCircle;

    public AnimationCurve widthOverHeight;

    public int[] costLadder; //Sequence of "ramping up" energy costs

    float fireCooldownLeft = 0f;
    public float buildTimeLeft;
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
        slider = towerGO.transform.FindChild("TowerCanvas").FindChild("Slider").GetComponent<Slider>();
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
            material.SetFloat("_ConstructGap", scanWidth);
        }
        //PrefabUtility.SetPropertyModifications(bulletPrefab, currentCostIndex);

        //Next two lines are a hack, will need fixing
        towerHeight = GetComponent<BoxCollider>().size.y; //total height of tower
        scanStartY = platformHeight; //lowest tower y - scan width

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
        #if !UNITY_EDITOR_LINUX
            AkSoundEngine.PostEvent ("Play_tower_build", gameObject);
        #endif

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
                material.SetFloat("_ConstructY", scanStartY - (t * (towerHeight + scanWidth * 2)) + towerHeight + (scanWidth * 2 * towerHeight));
            }

            if((1 - t) * (towerHeight + scanWidth * 2) < towerHeight) {
                laserEffect.setHeight(Mathf.Clamp((1 - t) * (towerHeight + scanWidth * 2), 0, towerHeight));
            }
            else {
                laserEffect.endConstruction(true);
            }
            laserEffect.setWidth(widthOverHeight.Evaluate(1 - t));
            yield return null;
        }

        laserEffect.vanish();
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

    public bool IsDestroyed()
    {
        return rubbleGO.activeInHierarchy;
    }

	public void Explode()
	{
		if (explosionPrefab) // was a prefab set in inspector?
		{
			//Debug.Log("creating explosionPrefab");
			GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
			Destroy(explosion,5); // FIXME: reuse pool
		}
	}

	public void Die()
    {
		Debug.Log("die tower");
		Explode();
        parentSpot.DestroyTower();
        rubbleGO.SetActive(true);
        towerGO.SetActive(false);
    }

    void OnDestroy() {
        Debug.Log("ondestroy tower");
        // Animate/place laser emitter
        if (building) {
            Debug.Log("endconstruction");
            parentSpot.GetComponent<LaserBuilderEffect>().endConstruction(false);
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

        if (IsDestroyed())
        {
            // Don't do anything while destroyed.
            return;
        }

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        Enemy nearestEnemy = null;

        float dist = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (d <= range && (nearestEnemy == null || d < dist))
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
        rubbleGO.transform.rotation = barrel.rotation;

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

		if (muzzleflashPrefab)
		{
			GameObject muzzy = Instantiate(muzzleflashPrefab, muzzles[muzzleIndex].position, muzzles[muzzleIndex].rotation);
			Destroy(muzzy,1); // FIXME: reuse pool
		}

		bulletGO.transform.SetParent(projectileGroup);

        bulletGO.GetComponent<ProjectileBase>().SetTarget(enemy.transform);

        muzzleIndex++;
        if (muzzles.Length <= muzzleIndex)
        {
            muzzleIndex = 0;
        }
    }

    public void Selected()
    {
        if (rubbleGO.activeInHierarchy)
        {
            return;
        }
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
