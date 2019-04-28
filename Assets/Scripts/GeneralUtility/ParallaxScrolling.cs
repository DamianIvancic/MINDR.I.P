using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {

    public float ParallaxSpeed;
    public float BlockSize;

    private Transform _camera;
    private Transform _background;
    private Transform[] _backgroundBlocks;

    private int _leftIdx;
    private int _rightIdx;
    private float _lastCameraX;
  
	void Start ()
    {
        _camera = Camera.main.transform;

        _background = transform;
        _backgroundBlocks = new Transform[_background.childCount];
        for(int i = 0; i<_background.childCount; i++)
        {
            _backgroundBlocks[i] = _background.GetChild(i);
        }
        _leftIdx = 0;
        _rightIdx = _background.childCount - 1;
	}
	

	void Update ()
    {
        float deltaX = _camera.position.x - _lastCameraX;
        _background.position = new Vector3(_background.position.x + deltaX * ParallaxSpeed, _background.position.y, _background.position.z);

        _lastCameraX = _camera.position.x;

        if (_camera.position.x < _backgroundBlocks[_leftIdx].position.x + BlockSize/2)
            ScrollLeft();

        if (_camera.position.x > _backgroundBlocks[_rightIdx].position.x - BlockSize/2)
            ScrollRight();   
    }

    void ScrollLeft()
    {
        _backgroundBlocks[_rightIdx].position = _backgroundBlocks[_leftIdx].position - Vector3.right * BlockSize;

        _leftIdx = _rightIdx;
        _rightIdx--;
        if (_rightIdx < 0)
            _rightIdx = _backgroundBlocks.Length - 1;
    }

    void ScrollRight()
    {
        _backgroundBlocks[_leftIdx].position = _backgroundBlocks[_rightIdx].position + Vector3.right * BlockSize;

        _rightIdx = _leftIdx;
        _leftIdx++;
        if (_leftIdx == _backgroundBlocks.Length)
            _leftIdx = 0;
    }
}
