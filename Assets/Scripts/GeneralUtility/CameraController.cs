using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {


    private Transform _player;
    private Transform _transform;

    private Vector3 _lastPos;

    private int _sceneIdx;
    private float _maxPosX;

    void Start()
    {
        _player = GameManager.GM.Player.transform;
        _transform = transform;
        transform.position = new Vector3(-9.5f, 1.5f, -5f);

        _sceneIdx = SceneManager.GetActiveScene().buildIndex;

        switch(_sceneIdx)
        {
            case (1):
                _maxPosX = 129;
                break;
            case (2):
                _maxPosX = 259;
                break;
            case (3):
                _maxPosX = 300;
                break;
        }
    }
	
	void Update ()
    {

        if(_player.position.x < -9.5)
        {
            Vector3 pos = new Vector3(-9.5f, transform.position.y, transform.position.z);
            _transform.position = pos;
        }
        else if(_player.position.x > _maxPosX)
        {
            Vector3 pos = new Vector3(_maxPosX, transform.position.y, transform.position.z);
            _transform.position = pos;
        }
        else
        {
            Vector3 pos = new Vector3(_player.position.x, transform.position.y, transform.position.z);
            _transform.position = pos;
        }

        //transform.position += new Vector3( Input.GetAxis("Horizontal"), 0, 0);
	}
}
