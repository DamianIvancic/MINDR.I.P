using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroZone : MonoBehaviour 
{
    private Enemy _owner; //no need for position updating since the zone is part of the same object as the owner

    void Start ()
    {
        _owner = transform.parent.GetComponentInChildren<Enemy>();
	}

    void OnTriggerStay2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            _owner.SetAggro(true);
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.tag == "Player")
        {
            _owner.SetAggro(false);
        }
    }
}
