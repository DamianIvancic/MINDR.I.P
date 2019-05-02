using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            enemy.DamageEnemy(collision);
            enemy.SetAggro(true);
            enemy.aggroLeashTimer = 0.0f;
            GetComponentInParent<Animator>().SetTrigger("Hit");
            
        }
    }
}
