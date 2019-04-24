using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Transform GroundCheckOrigin;
    public Transform GroundCheckFront;
    public Transform GroundCheckMid;
    public LayerMask GroundLayerMask;


    private bool _isGrounded;
    private float _gravity = -9.81f;
    private Transform _transform;
    private Rigidbody2D _rb;

    void Awake()
    {
        _transform = transform;
        _rb = GetComponentInChildren<Rigidbody2D>();
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 temp = new Vector2(_rb.velocity.x, _gravity);
        _rb.velocity = temp;

        _isGrounded = (Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckMid.position, GroundLayerMask));

        //Checks if it is on the edge of a platform
        if (!Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckFront.position, GroundLayerMask) && _isGrounded)
        {
            Debug.Log("I will turn around now.");
            Vector3 localScale = _transform.localScale;
            localScale.x *= -1.0f;
            _transform.localScale = localScale;
        }
        else
        {
            Debug.Log("I can walk forward.");
        }
	}
}
