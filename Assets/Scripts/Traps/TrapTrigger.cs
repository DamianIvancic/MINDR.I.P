using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public enum TrapType
    {
        Spike,
        Push
    }

    public TrapType type;

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            _anim.SetBool("Triggered", true);

            if (type == TrapType.Spike)
                HealthManager.Instance.TakeDamage(1);
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            _anim.SetBool("Triggered", false);
        }
    }
}
