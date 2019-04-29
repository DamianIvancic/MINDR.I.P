using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour {

    public string Message;

    private bool _displayed;

   
	void Update ()
    {

        if (_displayed && Input.GetKeyDown(KeyCode.Return))
            UIManager.Instance.FinishTextDisplay();
        
	}

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player" && _displayed == false)
        {
            UIManager.Instance.DisplayText(Message);
            _displayed = true;
        }
    }
}
