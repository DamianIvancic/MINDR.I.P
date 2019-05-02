using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource swingSound;
    public AudioSource damagedSound;
    [HideInInspector]
    public Rigidbody2D RB;
    [HideInInspector]
    public Animator anim;

    public Transform GroundCheckOrigin; // the origin from which the lines are projected (since the transform's position doesn't fit the sprite exactly)
    public List<Transform> GroundCheckList; //the points towards which the lines are projected
    public LayerMask GroundLayerMask;
    public LayerMask PlatformLayerMask;
    public bool isGrounded;

    public float Speed;
    public float JumpForce;
    public bool dashReady = true;
    public bool controllable = true;  //gets set to false when taking damage in midair until you land
    private bool _runToggled = false;
    private float _movementH;
    private float _movementV;
    private Vector3 _scale;

    private int _playerLayer;
    private int _platformLayer;

  
    private bool _isSwinging;
    private float _swingTimer = 0f;
    private float _swingCooldown = 1f;

    
    void Awake()
    {
        _playerLayer = gameObject.layer;
        _platformLayer = LayerMask.NameToLayer("Platform");

        RB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _scale = transform.localScale;

    }

	void Start ()
    {
        GameManager.GM.OnSetStateCallback += SetStateListener;
        TestInputManager.Instance.RegisterCallbacks();  
    
       //InputManager.Instance.RegisterCallbacks();         //registered from here instead of inside InputManager since there might not be a PlayerController active when the InputManager is instantiated
    }


    void Update ()
    {
        if (GameManager.GM.CurrentSate == GameManager.GameState.Playing)
        {
            UpdateMovement();
            UpdateAnimator();
            SetOrientation();
        }
    }

    void OnDestroy()
    {
        TestInputManager.Instance.ClearCallbacks();
        GameManager.GM.OnSetStateCallback -= SetStateListener;
       // InputManager.Instance.ClearCallbacks();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(LayerMask.LayerToName(collision.gameObject.layer) == "Ground" || LayerMask.LayerToName(collision.gameObject.layer) == "Platform")
        {
          
        }
    }

    void UpdateMovement()
    {
        if ( !anim.GetBool("Blocking") && !anim.GetBool("FireBreathing") && 
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Stomp") && !anim.GetCurrentAnimatorStateInfo(0).IsName("AirDash"))
        {
            UpdateJump();

            if(controllable)
            {
                if (_runToggled)
                    RB.velocity = new Vector2(_movementH * Speed * 1.5f, _movementV);
                else
                    RB.velocity = new Vector2(_movementH * Speed, _movementV);
            }
     
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

      //  Physics2D.IgnoreLayerCollision(_playerLayer, _platformLayer, Input.GetKey(KeyCode.S) || _movementV > 0f);

        for (int i = 0; i < GroundCheckList.Count; i++) //check if there anything between the player and the points slightly below his feet , if there is he's grounded
        {
            isGrounded = ((Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckList[i].position, GroundLayerMask) && RB.velocity.y == 0)
                         ||Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckList[i].position, PlatformLayerMask));
            if (isGrounded == true)
            {
                controllable = true;
                break;
            }
        }
    }

    void UpdateAnimator()
    {
        anim.SetBool("Grounded", isGrounded);

        if (RB.velocity.x != 0 && RB.velocity.y == 0)
            anim.SetBool(_runToggled ? "Running" : "Walking", true);
        else if(RB.velocity.x == 0 || !isGrounded)
            anim.SetBool(_runToggled ? "Running" : "Walking", false);

        if (!isGrounded && RB.velocity.y > 0)
            anim.SetBool("Jumping", true);
        else if (!isGrounded && RB.velocity.y < 0)
            anim.SetBool("Falling", true);
        else
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
        }  
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


    #region Callback listeners - > All the listener functions go here

    //Listener for GameManager SetState()
    void SetStateListener(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing)
        {
            anim.enabled = true;
            RB.gravityScale = 2.5f;
        }
        else
        {
            anim.enabled = false;
            RB.velocity = Vector2.zero;
            RB.gravityScale = 0;
            _movementH = 0;
            _movementV = 0;
        }
    }

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
        if (isGrounded)
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
        if(controllable)
         anim.SetBool("Attacking", true);
    }

    public void Dash()
    {
        if (controllable && dashReady)
        {
            anim.SetBool("Dashing", true);
            dashReady = false;  //gets set back to true when player enters Idle state (all transitions between states go through idle)
        }
    }

    public void Stomp()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")
           || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if(CooldownManager.Instance.StompSkillActive && !CooldownManager.Instance.StompCDActive)
                 anim.SetBool("Stomping", true);
        }
    }

    public void Block()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            anim.SetBool("Blocking", true);
        }   
    }

    public void StopBlock()
    {
        anim.SetBool("Blocking", false);
    }

    public void FireBreath()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")
          || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (CooldownManager.Instance.FireSkillActive && !CooldownManager.Instance.FireCDActive)      
                anim.SetBool("FireBreathing", true);                   
        }
    }

    public void StopFireBreath()
    {
        anim.SetBool("FireBreathing", false);
    }

    public void ToggleRun()
    {
        anim.SetBool(_runToggled ? "Running" : "Walking", false);
        _runToggled = !_runToggled;    
    }



    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.localScale.x > 0 ? transform.position + Vector3.right  : transform.position - Vector3.right , 0.1f);
        Gizmos.DrawWireSphere(transform.localScale.x > 0 ? transform.position + new Vector3(1,-1.25f,0) : transform.position - Vector3.right, 0.1f);
    }
}
