using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float JumpForce;

    private bool _runToggled = false;
    private float _movementH;
    private float _movementV;
    private Vector3 _scale;

    private int _playerLayer;
    private int _platformLayer;

    private bool _isGrounded;
    public Transform GroundCheckOrigin; // the origin from which the lines are projected (since the transform's position doesn't fit the sprite exactly)
    public List<Transform> GroundCheckList; //the points towards which the lines are projected
    public LayerMask GroundLayerMask;

    private bool _isSwinging;
    private float _swingTimer = 0f;
    private float _swingCooldown = 1f;

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
        _scale = transform.localScale;

    }

	void Start ()
    {
        InputManager.Instance.RegisterCallbacks();         //registered from here instead of inside InputManager since there might not be a PlayerController active when the InputManager is instantiated
	}
	

	void Update ()
    {
        UpdateMovement();
        UpdateAnimator();
        SetOrientation();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(LayerMask.LayerToName(collision.gameObject.layer) == "Ground" || LayerMask.LayerToName(collision.gameObject.layer) == "Platform")
        {
            //Debug.Log("collision!" + collision.gameObject.name);
        }
    }

    void UpdateMovement()
    {
        if (!_anim.GetBool("Blocking") && !_anim.GetBool("FireBreathing") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            UpdateJump();

            if (_runToggled)
                RB.velocity = new Vector2(_movementH * Speed * 1.5f, _movementV);
            else
                RB.velocity = new Vector2(_movementH * Speed, _movementV);

            /*if (RB.velocity.magnitude == 0f)
                 RB.isKinematic = true; //prevents enemies from pushing the player
             else
                 RB.isKinematic = false;*/
        }
        else
            RB.velocity = Vector2.zero;
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
        _anim.SetBool("Grounded", _isGrounded);

        if (RB.velocity.x != 0 && RB.velocity.y == 0)
            _anim.SetBool(_runToggled ? "Running" : "Walking", true);
        else if(RB.velocity.x == 0 || !_isGrounded)
            _anim.SetBool(_runToggled ? "Running" : "Walking", false);

        if (!_isGrounded && RB.velocity.y > 0)
            _anim.SetBool("Jumping", true);
        else if (!_isGrounded && RB.velocity.y < 0)
            _anim.SetBool("Falling", true);
        else
        {
            _anim.SetBool("Jumping", false);
            _anim.SetBool("Falling", false);
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

    void SetOrientation()
    {
        if (RB.velocity.x < 0)
        {
            Vector3 scale = _scale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else if (RB.velocity.x > 0)
        {
            transform.localScale = _scale;
        }
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
        if (_isGrounded)
        {
            RB.AddForce(Vector2.up * JumpForce);
        }
    }

    public void StopJump()
    {
        if (_movementV > 0f)
        {
            Vector2 velocity = RB.velocity;
            velocity.y = 0f;
            RB.velocity = velocity;
        }
    }

    public void Attack()
    {
        _anim.SetTrigger("Attacking");   
    }

    public void Dash()
    {
        _anim.SetTrigger("Dashing");
    }

    public void Stomp()
    {
        _anim.SetTrigger("Stomping");
    }

    public void Block()
    {
        _anim.SetBool("Blocking", true);
    }

    public void StopBlock()
    {
        _anim.SetBool("Blocking", false);
    }

    public void FireBreath()
    {
        _anim.SetBool("FireBreathing", true);
    }

    public void StopFireBreath()
    {
        _anim.SetBool("FireBreathing", false);
    }

    public void ToggleRun()
    {
        _anim.SetBool(_runToggled ? "Running" : "Walking", false);
        _runToggled = !_runToggled;    
    }

    #endregion

    void OnDrawGizmosSelected()
    {

    }
}
