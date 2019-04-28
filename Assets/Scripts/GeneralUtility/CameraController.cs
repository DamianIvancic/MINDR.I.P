using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    private Transform _player;
    private Transform _transform;

    private Vector3 _lastPos;

    void Start()
    {
        _player = GameManager.GM.Player.transform;
        _transform = transform;
    }
	

	void Update ()
    {

        if(_player.position.x > -16.5)
        {
            Vector3 pos = new Vector3(_player.position.x, transform.position.y, transform.position.z);
            _transform.position = pos;
        }
        else
        {
            Vector3 pos = new Vector3(-16.5f, transform.position.y, transform.position.z);
            _transform.position = pos;
        }


       // transform.position += new Vector3( Input.GetAxis("Horizontal"), 0, 0);
	}
}
