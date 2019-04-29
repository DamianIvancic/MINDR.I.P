﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWizard : Enemy {

    public Transform GroundCheckOrigin;
    public Transform GroundCheckFront;
    public Transform GroundCheckMid;
    public LayerMask GroundLayerMask;

    [HideInInspector]
    public StateMachine<SkeletalWizard> stateMachine;
    [HideInInspector]
    public bool aggro = false; // Did it see the player in the last few seconds
    [HideInInspector]
    public float aggroLeashDuration = 5.0f; // Defines the 'few' seconds
    [HideInInspector]
    public float aggroLeashTimer = 6.0f;
    [HideInInspector]
    public bool stateFinished; // used as a check for transitioning between some states. set by the states updates but also the animator script CrawlerSwing when the animation ends
    public bool _isGrounded; // Is it touching the ground?
    [HideInInspector]
    public float _gravity = -9.81f; // How fast it gets pulled down
    [HideInInspector]
    public bool _isTurnedLeft = true;
    [HideInInspector]
    public float speed = 5.0f;
    [HideInInspector]
    public Transform _transform;
    [HideInInspector]
    public Rigidbody2D _rb;

    private Transform _player;
    private Rigidbody2D _playerRB;

    void Awake()
    {
        stateMachine = new StateMachine<SkeletalWizard>(this);
        stateMachine.ChangeState(SkeletalWizardMove.Instance);
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start ()
    {
        _player = GameManager.GM.Player.transform;
        _playerRB = _player.GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        _isGrounded = (Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckMid.position, GroundLayerMask));

        if (GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            stateMachine.Update();
        }

        

        //Checks if it is on the edge of a platform
        
        

        if (aggroLeashTimer < aggroLeashDuration)
        {
            SetAggro(true);
            aggroLeashTimer += Time.deltaTime;
        }
        else
        {
            SetAggro(false);
        }
	}

    public override void SetAggro(bool state)
    {
        aggro = state;
    }
}