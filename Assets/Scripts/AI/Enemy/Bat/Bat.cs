using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [HideInInspector]
    public StateMachine<Bat> stateMachine;
 
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

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
            HealthManager.Instance.TakeDamage(damage);

        if (trigger.gameObject.tag == "Weapon")    
            TakeDamage();     
    }

    protected override void TriggerDeath()
    {
        _particles.Play();
        creature.SetActive(false);
        Destroy(gameObject, 1f);
    }
}
