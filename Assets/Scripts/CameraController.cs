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

        if(_player.position.x > -18)
        {
            Vector3 pos = new Vector3(_player.position.x, _transform.position.y, transform.position.z);
            _transform.position = pos;
        }
        else
        {
            Vector3 pos = new Vector3(-18, _transform.position.y, transform.position.z);
            _transform.position = pos;
        }

	}
}
