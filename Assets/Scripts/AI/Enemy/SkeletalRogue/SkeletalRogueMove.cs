using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalRogueMove : State<SkeletalRogue>
{
    private static SkeletalRogueMove _instance;

    //states this can transit into
    private static SkeletalRogueAggroed _aggroStateReference;
    private static SkeletalRogueDeath _deathStateReference;

    public static SkeletalRogueMove Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalRogueMove();
            }

            return _instance;
        }
    }

    public SkeletalRogueMove()
    {
        if (_instance != null)
            return;
        _instance = this;
        _aggroStateReference = SkeletalRogueAggroed.Instance;
        _deathStateReference = SkeletalRogueDeath.Instance;
    }

    public override void EnterState(SkeletalRogue owner)
    {
        Debug.Log("State enter: " + this);
    }

    public override void UpdateState(SkeletalRogue owner)
    {
        UpdateAI(owner);
        UpdateMovement(owner);
        if (owner.stateMachine.currentState == this)
            UpdateAnimator(owner);
    }

    public override void UpdateAI(SkeletalRogue owner)
    {
        if (owner.isDead)
            owner.stateMachine.ChangeState(_deathStateReference);
        if (owner.aggro)
            owner.stateMachine.ChangeState(_aggroStateReference);
    }

    public override void UpdateMovement(SkeletalRogue owner)
    {
        if (!owner.isStunned)
        {
            if (!Physics2D.Linecast(owner.GroundCheckOrigin.position, owner.GroundCheckFront.position, owner.GroundLayerMask) && owner._isGrounded)
            {
                Debug.Log("I will turn around now.");
                owner.TurnAround();
            }
            else
            {
                Debug.Log("I can walk forward.");
                if (owner.transform.localScale.x > 0) //turned left 
                {
                    Vector2 temp = new Vector2(-1 * owner.speed, owner._gravity);
                    owner.RB.velocity = temp;
                }
                else //turned right
                {
                    Vector2 temp = new Vector2(1 * owner.speed, owner._gravity);
                    owner.RB.velocity = temp;
                }
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
        if (owner.RB.velocity.magnitude > 0f)     
            owner.anim.SetBool("IsWalking", true);    
        else    
            owner.anim.SetBool("IsWalking", false);     
    }

    public override void ExitState(SkeletalRogue owner)
    {
        Debug.Log("State exit: " + this);
    }
}
