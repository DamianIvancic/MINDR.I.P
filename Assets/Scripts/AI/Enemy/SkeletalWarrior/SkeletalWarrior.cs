using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWarrior : Enemy {

    [HideInInspector]
    public StateMachine<SkeletalWarrior> stateMachine;

    public LayerMask GroundLayerMask;
    public Transform GroundCheckOrigin;
    public Transform GroundCheckFront;
    public Transform GroundCheckMid;
    
    private Transform _player;
    private Rigidbody2D _playerRB;

    public float attackCooldown = 3.5f;
    public float attackTimer = 3.5f;
    [HideInInspector]
    public float _gravity = -9.81f; // How fast it gets pulled down
    public bool _isGrounded; // Is it touching the ground?


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<SkeletalWarrior>(this);
        stateMachine.ChangeState(SkeletalWarriorMove.Instance);
        RB = GetComponent<Rigidbody2D>();
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
        if (GameManager.GM.CurrentSate == GameManager.GameState.Playing)
        {
            base.Update();

            _isGrounded = (Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckMid.position, GroundLayerMask));

            attackTimer += Time.deltaTime;

            if (aggroLeashTimer < aggroLeashDuration)
            {
                aggro = true;
                aggroLeashTimer += Time.deltaTime;
            }
            else     
                SetAggro(false);
            
            //keep this last in the update
            stateMachine.Update();
        }
    }

    public override void SetAggro(bool state)
    {
        base.SetAggro(state);

        if (state == true)
            aggroLeashTimer = 0f;
    }

    public void TurnAround()
    {
        Vector3 localScale = _transform.localScale;
        localScale.x *= -1.0f;
        _transform.localScale = localScale;
    }

    protected override void TriggerDeath()
    {
        //_particles.Play();
        //creature.SetActive(false);
        Destroy(gameObject, 3f);
        isDead = true;
    }
}
