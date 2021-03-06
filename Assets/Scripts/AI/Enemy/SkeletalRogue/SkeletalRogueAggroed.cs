﻿using System.Collections;
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
        owner.speed *= owner.meleeSpeedIncrease;

        Debug.Log("State enter: " + this);
    }

    public override void UpdateState(SkeletalRogue owner)
    {
        Debug.Log("RogueAggroed");

        UpdateAI(owner);
        if (owner.stateMachine.currentState == this)
        {
            UpdateMovement(owner);
            UpdateAnimator(owner);
        }
    }

    public override void UpdateAI(SkeletalRogue owner)
    {
        if (owner.isDead)
            owner.stateMachine.ChangeState(_deathStateReference);
        if (!owner.aggro)
            owner.stateMachine.ChangeState(_moveStateReference);
    }

    public override void UpdateMovement(SkeletalRogue owner)
    {
        if (!owner.isStunned)
        {
            //Turn to follow player
            if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 0)
            {
                if (owner.transform.localScale.x < 0 ) owner.TurnAround(); //turn around if facing right
            }
            else
            {
                if (owner.transform.localScale.x > 0) owner.TurnAround(); //turn around if facing left
            }

            //Dont move if player in your range
            if (owner.playerInRange)
            {
                owner.RB.velocity = new Vector2(0, owner._gravity);
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 2f)
            {
                Vector2 temp = new Vector2(-1 * owner.speed, owner._gravity);
                owner.RB.velocity = temp;
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x < -2f)
            {
                Vector2 temp = new Vector2(1 * owner.speed, owner._gravity);
                owner.RB.velocity = temp;
            }
            else
            {
                owner.RB.velocity = new Vector2(0, owner._gravity);
            }
        }
        else
        {
            Debug.Log("I am hit Stunned.");
            Vector2 temp = new Vector2(0, owner._gravity);
            owner.RB.velocity = temp;
        }
    }

    public override void UpdateAnimator(SkeletalRogue owner)
    {
        if (!owner.isStunned)
        {
            if (owner.playerInRange)
            {
                if (owner.attackTimer > owner.attackCooldown)
                {
                    owner.anim.SetTrigger("Attack");
                    owner.attackTimer = 0f;
                }
                owner.anim.SetBool("IsWalking", false);
            }
            else if (Mathf.Abs(owner.transform.position.x - GameManager.GM.Player.transform.position.x ) > 2f)
            {
                owner.anim.SetBool("IsWalking", true);
            }
            else
            {
                owner.anim.SetBool("IsWalking", false);
            }
        }
        else
        {
            owner.anim.SetBool("IsWalking", false);
        }
    }

    public override void ExitState(SkeletalRogue owner)
    {
        Debug.Log("State exit: " + this);
    }
}
