using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy {

    [HideInInspector]
    public StateMachine<Ghost> stateMachine;
 
    [HideInInspector]
    public SpriteRenderer sprite;

    [HideInInspector]
    public Color color;

    private float _acceleration = 1.5f;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        color = sprite.color;

        stateMachine = new StateMachine<Ghost>(this);
        stateMachine.ChangeState(GhostIdle.Instance);
    
        startingPos = transform.position;
    }

    protected override void Update ()
    {
        if (GameManager.GM.CurrentSate == GameManager.GameState.Playing)
        {
            speed += _acceleration * Time.deltaTime;

            if (speed > 7.5f)
                speed = 7.5f;

            stateMachine.Update();
        }
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player" && speed > 0.2f)
        {           
            anim.SetTrigger("Attacking");
            HealthManager.Instance.TakeDamage(damage);
        }
    }

    public override void SetAggro(bool state)
    {
        aggro = state;
        if (aggro == true)
            aggroZone.SetActive(false);
    }
}
