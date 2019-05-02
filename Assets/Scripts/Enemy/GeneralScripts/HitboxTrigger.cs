using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<Enemy>().DamageEnemy(collision);
    }
}
