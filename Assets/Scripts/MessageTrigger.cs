using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour {

    public string Message;

    private bool _displayed;

	void Start () {
		
	}
	
	void Update ()
    {
        if (_displayed && Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.Instance.TextBackground.gameObject.SetActive(false);
            Destroy(gameObject);           
        }
	}

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            UIManager.Instance.DisplayText(Message);
            _displayed = true;
        }
    }
}
