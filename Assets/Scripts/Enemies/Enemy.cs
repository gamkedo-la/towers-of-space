using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float health = 1f;

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
            Die ();
        }
    }

    public void ReachedGoal() {
        // @todo
        UIController.instance.LoseLife(); //instance is the Score Manager
        Die ();
    }

    public void Die() {
        // @todo particles/explosion
        Destroy(gameObject);
    }
}
