using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warlock : Enemy
{
    [HideInInspector]
    public StateMachine<Warlock> stateMachine;

    public Transform RetreatA;
    public Transform RetreatB;
    [HideInInspector]
    public Vector3 targetPos;

    public float attackCooldown = 5f;
    public float attackTimer = 5f;

    [HideInInspector]
    public bool invulnerable;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<Warlock>(this);
        stateMachine.ChangeState(WarlockIdle.Instance);
    }

    protected override void Update()
    {

        if (GameManager.GM.CurrentSate == GameManager.GameState.Playing)
        {
            base.Update();
           
            attackTimer += Time.deltaTime;

            if (aggroLeashTimer < aggroLeashDuration)
            {
                aggro = true;
                aggroLeashTimer += Time.deltaTime;
            }
            else
            {
                SetAggro(false);
            }

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

    public override bool TakeDamage(int damage = 1)
    {
        SetAggro(true);

        if (isDead == false && !invulnerable)
        {      
            anim.SetTrigger("Hit");
            _damagedSound.Play();
            HealthManager.Instance.ChargeBerserk();

            _currentHP -= damage;
             if (_currentHP <= 0)
                TriggerDeath();
             else 
                stateMachine.ChangeState(WarlockRetreat.Instance);

            return true;        
        }
        return false;
    }

    public void TurnAround()
    {
        Vector3 localScale = gameObject.transform.localScale;
        localScale.x *= -1.0f;
        transform.localScale = localScale;
    }

    protected override void TriggerDeath()
    {       
        isDead = true;
        StartCoroutine(GameManager.GM.LoadScene(4, 3.5f));
    }

}
