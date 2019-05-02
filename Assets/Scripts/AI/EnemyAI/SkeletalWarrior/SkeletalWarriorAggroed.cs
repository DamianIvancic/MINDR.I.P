using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWarriorAggroed : State<SkeletalWarrior>
{

    private static SkeletalWarriorAggroed _instance;

    //states this can transit into
    private static SkeletalWarriorMove _moveStateReference;
    private static SkeletalWarriorDeath _deathStateReference;

    public static SkeletalWarriorAggroed Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalWarriorAggroed();
            }

            return _instance;
        }
    }

    public SkeletalWarriorAggroed()
    {
        if (_instance != null)
            return;
        _instance = this;
        _moveStateReference = SkeletalWarriorMove.Instance;
        _deathStateReference = SkeletalWarriorDeath.Instance;
    }

    public override void EnterState(SkeletalWarrior owner)
    {
        owner.stateFinished = false;
        owner.speed *= owner.meleeSpeedIncrease;

        Debug.Log("State enter: " + this);
    }

    public override void ExitState(SkeletalWarrior owner)
    {
        owner.speed /= owner.meleeSpeedIncrease;
        Debug.Log("State exit: " + this);
    }

    public override void UpdateAI(SkeletalWarrior owner)
    {
        if (owner.isDead)
            owner.stateMachine.ChangeState(_deathStateReference);
        if (!owner.aggro)
            owner.stateMachine.ChangeState(_moveStateReference);
    }

    public override void UpdateAnimator(SkeletalWarrior owner)
    {
        if (!owner.isStunned)
        {
            if (owner.isInRange)
            {
                if (owner.attackTimer > owner.attackCooldown)
                {
                    owner._anim.SetTrigger("Attack");
                    owner.attackTimer = 0f;
                }
                owner._anim.SetBool("IsWalking", false);
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 2f)
            {
                owner._anim.SetBool("IsWalking", true);
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x < -2f)
            {
                owner._anim.SetBool("IsWalking", true);
            }
            else
            {
                owner._anim.SetBool("IsWalking", false);
            }
        }
        else
        {
            owner._anim.SetBool("IsWalking", false);
        }
    }

    public override void UpdateMovement(SkeletalWarrior owner)
    {
        if (!owner.isStunned)
        {

            //Turn to follow player
            if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 0)
            {
                if (!owner._isTurnedLeft) owner.TurnAround();
            }
            else
            {
                if (owner._isTurnedLeft) owner.TurnAround();
            }

            //Dont move if player in your range
            if (owner.isInRange)
            {
                owner._rb.velocity = new Vector2(0, owner._gravity);
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 2f)
            {
                Vector2 temp = new Vector2(-1 * owner.speed, owner._gravity);
                owner._rb.velocity = temp;
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x < -2f)
            {
                Vector2 temp = new Vector2(1 * owner.speed, owner._gravity);
                owner._rb.velocity = temp;
            }
            else
            {
                owner._rb.velocity = new Vector2(0, owner._gravity);
            }
        }
        else
        {
            Debug.Log("I am hit Stunned.");
            Vector2 temp = new Vector2(0, owner._gravity);
            owner._rb.velocity = temp;
        }
    }

    public override void UpdateState(SkeletalWarrior owner)
    {
        UpdateAI(owner);
        if (owner.stateMachine.currentState == this)
        {
            UpdateMovement(owner);
            UpdateAnimator(owner);
        }
    }
}
