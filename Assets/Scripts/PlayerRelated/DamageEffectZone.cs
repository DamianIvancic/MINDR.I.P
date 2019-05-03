using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectZone : MonoBehaviour {

    public int damage;

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Enemy")
        {
            Enemy enemy = trigger.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
