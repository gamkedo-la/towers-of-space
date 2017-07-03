using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	public float health = 1f;
	public int lootEnergy;
	float startHealth;
	Slider slider;
	public GameObject explosionPrefab;

	void Start()
	{
		slider = transform.FindChild("EnemyCanvas").FindChild("Slider").GetComponent<Slider>();
		startHealth = health;
	}

	public void TakeDamage(float damage)
	{
		health -= damage;

		slider.value = health / startHealth;

		if (health <= 0)
		{
			GiveLootEnergy();
			Die();
		}
	}

	public void ReachedGoal()
	{
		Destroy(gameObject);
		GameController.instance.LoseLife(); //instance is the Score Manager
	}

	public void GiveLootEnergy()
	{
		GameController.instance.AddEnergy(lootEnergy);
	}

	public void Explode()
	{
		if (explosionPrefab) // was a prefab set in inspector?
		{
			//Debug.Log("creating explosionPrefab");
			GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(explosion, 5); // FIXME: reuse pool
		}
	}

	public void Die()
	{
		Explode();
		Destroy(gameObject);
		#if !UNITY_EDITOR_LINUX
			AkSoundEngine.PostEvent ("Play_tank_explo", gameObject);
		#endif
	}
}
