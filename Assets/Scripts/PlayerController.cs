using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float JumpForce;

    private float _movementH;
    private float _movementV;
    private Vector2 _movement;

    private int _playerLayer;
    private int _platformLayer;

    private bool _isGrounded;
    public Transform GroundCheckOrigin; // the origin from which the lines are projected (since the transform's position doesn't fit the sprite exactly)
    public List<Transform> GroundCheckList; //the points towards which the lines are projected
    public LayerMask GroundLayerMask;

    private bool _isSwinging;
    private float _swingTimer = 0f;
    private float _swingCooldown;

    [HideInInspector]
    public Rigidbody2D RB;
    [HideInInspector]
    public AudioSource swingSound;
    private Animator _anim;


    void Awake()
    {
        _playerLayer = gameObject.layer;
        _platformLayer = LayerMask.NameToLayer("Platform");

        RB = GetComponent<Rigidbody2D>();
        swingSound = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();    
    }

	void Start () {
		
	}
	

	void Update ()
    {
		
	}

    void UpdateMovement()
    {
        UpdateJump();

        _movement = new Vector2(_movementH, _movementV);
        _movement.Normalize();

        RB.velocity = _movement * Speed;

        if (RB.velocity.magnitude == 0f)
            RB.isKinematic = true; //prevents enemies from pushing the player
        else
            RB.isKinematic = false;
    }

    void UpdateJump()
    {
        _movementV = RB.velocity.y;

        Physics2D.IgnoreLayerCollision(_playerLayer, _platformLayer, Input.GetKey(KeyCode.S) || _movementV > 0f);

        for (int i = 0; i < GroundCheckList.Count; i++) //check if there anything between the player and the points slightly below his feet , if there is he's grounded
        {
            _isGrounded = Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckList[i].position, GroundLayerMask);
            if (_isGrounded == true)
                break;
        }
    }

    void UpdateAnimator()
    {
        if (RB.velocity.magnitude > 0)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        SetOrientation();
    }

    void SetOrientation()
    {
        if (RB.velocity.x < 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", true);
        }
        else if (RB.velocity.x == 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", false);
        }
        else if (RB.velocity.x > 0)
        {
            _anim.SetBool("Right", true);
            _anim.SetBool("Left", false);
        }


     /*   if (RB.velocity.y < 0)  <-------------JUMP
        {
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", true);
        }
        else if (RB.velocity.y == 0)
        {
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", false);
        }
        else if (RB.velocity.y > 0)
        {
            _anim.SetBool("Up", true);
            _anim.SetBool("Down", false);
        }*/
    }


    #region Callbacks - > All the callback functions go here

    public void MoveLeft()
    {
        _movementH = -1;
    }

    public void MoveRight()
    {
        _movementH = 1;
    }

    public void StopMovingHorizontal()
    {
        _movementH = 0;
    }

    public void Jump()
    {
        if(_isGrounded)
          RB.AddForce(Vector2.up * JumpForce);
    }

    public void StopJump()
    {
        if (_movementV > 0f)
            _movementV = 0f;
    }

    public void Swing()
    {
        if (_swingTimer > _swingCooldown)
        {
            _anim.SetTrigger("IsSwinging");
            _swingTimer = 0f;
        }
    }

    #endregion
}
