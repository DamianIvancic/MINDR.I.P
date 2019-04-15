using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    private Transform _player;
    private Transform _transform;

    void Start()
    {
        _player = GameManager.GM.Player.transform;
        _transform = transform;
    }
	

	void Update ()
    {
        Vector3 pos = _player.transform.position;
        pos.z -= 10;
        _transform.position = pos;
	}
}
