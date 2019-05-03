using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
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
