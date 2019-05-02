using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [HideInInspector]
    public StateMachine<Bat> stateMachine;
    [HideInInspector]
    public Animator anim;
    
    [HideInInspector]
    public float frequency = 2f;
    [HideInInspector]
    public float magnitude = 5f;

    public float sineTimer;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponentInChildren<Animator>();

        stateMachine = new StateMachine<Bat>(this);
        if(transform.parent != null && transform.parent.gameObject.tag == "Spawner")
            stateMachine.ChangeState(BatFlying.Instance);
        else
            stateMachine.ChangeState(BatIdle.Instance);  
  
    }

    protected override void Update()
    {
        if (GameManager.GM.CurrentSate == GameManager.GameState.Playing)        
            stateMachine.Update();     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamagePlayer(collision);

        if (collision.gameObject.tag == "Weapon")
        {
            DamageEnemy(collision);
        }
    }

    protected override void Kill()
    {
        _particles.Play();
        creature.SetActive(false);
        Destroy(gameObject, 1f);
    }

}
