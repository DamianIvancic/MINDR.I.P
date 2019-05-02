using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWarrior : Enemy {

    public Transform GroundCheckOrigin;
    public Transform GroundCheckFront;
    public Transform GroundCheckMid;
    public LayerMask GroundLayerMask;

    [HideInInspector]
    public StateMachine<SkeletalWarrior> stateMachine;
    //[HideInInspector]
    //public bool aggro = false; // Did it see the player in the last few seconds
    
    [HideInInspector]
    public float attackCooldown = 6.0f;

    public float attackTimer = 6.0f;
    [HideInInspector]
    public bool stateFinished; // used as a check for transitioning between some states. set by the states updates but also the animator script CrawlerSwing when the animation ends
    public bool _isGrounded; // Is it touching the ground?
    
    [HideInInspector]
    public bool seesPlayer = false;
    [HideInInspector]
    public float _gravity = -9.81f; // How fast it gets pulled down
    [HideInInspector]
    public bool _isTurnedLeft = true;
    //[HideInInspector]
    //public float speed = 5.0f;
    //[HideInInspector]
    //public Transform _transform;
    [HideInInspector]
    public Rigidbody2D _rb;
    [HideInInspector]
    public Animator _anim;

    private Transform _player;
    private Rigidbody2D _playerRB;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<SkeletalWarrior>(this);
        stateMachine.ChangeState(SkeletalWarriorMove.Instance);
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
        _player = GameManager.GM.Player.transform;
        _playerRB = _player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();

        _isGrounded = (Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckMid.position, GroundLayerMask));

        attackTimer += Time.deltaTime;

        if (aggroLeashTimer < aggroLeashDuration)
        {
            SetAggro(true);
            aggroLeashTimer += Time.deltaTime;
        }
        else
        {
            SetAggro(false);
        }



        //keep this last in the update
        if (GameManager.GM.CurrentSate == GameManager.GameState.Playing)
        {
            stateMachine.Update();
        }
    }

    #region Triggers and Colliders

    void OnTriggerEnter2D(Collider2D collision)
    {
        //DamagePlayer(collision);


        if (collision.tag == "Player")
        {
            Debug.Log("The Player is in front of me.");
            seesPlayer = true;
            SetAggro(true);
            aggroLeashTimer = 0f;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            aggroLeashTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("The Player left my vision.");
            seesPlayer = false;
        }
    }

    #endregion

    public void TurnAround()
    {
        Vector3 localScale = _transform.localScale;
        localScale.x *= -1.0f;
        _transform.localScale = localScale;

        if (_isTurnedLeft) _isTurnedLeft = false;
        else _isTurnedLeft = true;

    }

    protected override void Kill()
    {
        //_particles.Play();
        //creature.SetActive(false);
        Destroy(gameObject, 3f);
        isDead = true;
    }
}
