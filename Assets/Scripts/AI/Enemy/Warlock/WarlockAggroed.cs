using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockAggroed : State<Warlock>
{
    private static WarlockAggroed _instance;

    //states this can transit into
    private static WarlockDeath _deathStateReference;

    public static WarlockAggroed Instance
    {
        get
        {
            if (_instance == null)
            {
                new WarlockAggroed();
            }

            return _instance;
        }
    }

    public WarlockAggroed()
    {
        if (_instance != null)
            return;
        _instance = this;
        _deathStateReference = WarlockDeath.Instance;
    }

    public override void EnterState(Warlock owner)
    {}

    public override void UpdateState(Warlock owner)
    {
        UpdateAI(owner);
        UpdateMovement(owner);
        if (owner.stateMachine.currentState == this)
            UpdateAnimator(owner);
    }

    public override void UpdateAI(Warlock owner)
    {
        if (owner.isDead)
            owner.stateMachine.ChangeState(_deathStateReference);
    }

    public override void UpdateMovement(Warlock owner)
    {
        if (!owner.isStunned)
        {
            //Turn to follow player
            if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 0)
            {
                if (owner.transform.localScale.x > 0) owner.TurnAround();
            }
            else
            {
                if (owner.transform.localScale.x < 0) owner.TurnAround();
            }

    
            if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 5.5f)
            {        
                owner.transform.position += Vector3.left * owner.speed * Time.deltaTime;
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x < -5.5f)
            {
                owner.transform.position += Vector3.right * owner.speed * Time.deltaTime;
            }
      
        }
    }

    public override void UpdateAnimator(Warlock owner)
    {
        if (!owner.isStunned)
        {        
            if (owner.attackTimer > owner.attackCooldown)
            {
                owner.anim.SetBool("Casting", true);                 
                owner.anim.SetBool("Walking", false);
            }
            else if (Mathf.Abs(owner.transform.position.x - GameManager.GM.Player.transform.position.x) > 5.5f)
            {
                owner.anim.SetBool("Walking", true);
            }
            else
            {
                owner.anim.SetBool("Walking", false);
            }
        }
        else
        {
            owner.anim.SetBool("Walking", false);
        }
    }

    public override void ExitState(Warlock owner)
    {
    }
}
