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
            if (enemy.hitInvulTimer > enemy.hitInvulDuration)
                GetComponentInParent<Animator>().SetTrigger("Hit");

            enemy.TakeDamage(); //set aggro gets called inside take damage          
        }
    }
}
