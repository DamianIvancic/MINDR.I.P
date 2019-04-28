using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{

    [HideInInspector]
    public StateMachine<Bat> stateMachine;
    [HideInInspector]
    public Animator anim;
    private BoxCollider2D _trigger;

    [HideInInspector]
    public Vector3 startingPos;

    //[HideInInspector]
    public float speed = 0.01f;
    [HideInInspector]
    public float frequency = 2f;
    [HideInInspector]
    public float magnitude = 4f;

    public float sineTimer;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<Bat>(this);
        stateMachine.ChangeState(BatIdle.Instance);
        startingPos = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.Update();
   
    }
}
