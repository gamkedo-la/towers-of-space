using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float health = 1f;
    public int lootEnergy;

    float startHealth;

    Slider slider;

    void Start()
    {
        slider = transform.FindChild("EnemyCanvas").FindChild("Slider").GetComponent<Slider>();
        startHealth = health;
    }
    
    public void TakeDamage(float damage) {
        health -= damage;

        slider.value = health / startHealth;

        if (health <= 0) {
            GiveLootEnergy();
            Die();
        }
    }

    public void ReachedGoal() {
        // @todo
        GameController.instance.LoseLife(); //instance is the Score Manager
        Die();
    }

    public void GiveLootEnergy() {
        GameController.instance.AddEnergy(lootEnergy);
    }

    public void Die() {
        // @todo particles/explosion
        Destroy(gameObject);
    }
}
