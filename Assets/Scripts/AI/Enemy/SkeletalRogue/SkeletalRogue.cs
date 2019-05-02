﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalRogue : Enemy
{
    [HideInInspector]
    public StateMachine<SkeletalRogue> stateMachine;

    public Transform GroundCheckOrigin;
    public Transform GroundCheckFront;
    public Transform GroundCheckMid;
    public LayerMask GroundLayerMask;

    private Transform _player;
    private Rigidbody2D _playerRB;

    public float attackCooldown = 2.5f;
    [HideInInspector]
    public float attackTimer = 2.5f;

    [HideInInspector]
    public float _gravity = -9.81f; // How fast it gets pulled down
    public bool _isGrounded; // Is it touching the ground?

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<SkeletalRogue>(this);
        stateMachine.ChangeState(SkeletalRogueMove.Instance);
       
        RB = GetComponent<Rigidbody2D>();    
    }

    void Start()
    {
        _player = GameManager.GM.Player.transform;
        _playerRB = _player.GetComponent<Rigidbody2D>();
    }

    protected override void Update()
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
