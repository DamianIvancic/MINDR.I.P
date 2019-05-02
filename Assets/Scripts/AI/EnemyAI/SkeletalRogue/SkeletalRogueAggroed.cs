using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalRogueAggroed : State<SkeletalRogue>
{

    private static SkeletalRogueAggroed _instance;

    //states this can transit into
    private static SkeletalRogueMove _moveStateReference;
    private static SkeletalRogueDeath _deathStateReference;

    public static SkeletalRogueAggroed Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalRogueAggroed();
            }

            return _instance;
        }
    }

    public SkeletalRogueAggroed()
    {
        if (_instance != null)
            return;
        _instance = this;
        _moveStateReference = SkeletalRogueMove.Instance;
        _deathStateReference = SkeletalRogueDeath.Instance;
    }

    public override void EnterState(SkeletalRogue owner)
    {
        owner.stateFinished = false;
        owner.speed *= owner.meleeSpeedIncrease;

        Debug.Log("State enter: " + this);
    }

    public override void ExitState(SkeletalRogue owner)
    {
        owner.speed /= owner.meleeSpeedIncrease;
        Debug.Log("State exit: " + this);
    }

    public override void UpdateAI(SkeletalRogue owner)
    {
        if (owner.isDead)
            owner.stateMachine.ChangeState(_deathStateReference);
        if (!owner.aggro)
            owner.stateMachine.ChangeState(_moveStateReference);
    }

    public override void UpdateAnimator(SkeletalRogue owner)
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

    public override void UpdateMovement(SkeletalRogue owner)
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

    public override void UpdateState(SkeletalRogue owner)
    {
        UpdateAI(owner);
        if (owner.stateMachine.currentState == this)
        {
            UpdateMovement(owner);
            UpdateAnimator(owner);
        }
    }
}
