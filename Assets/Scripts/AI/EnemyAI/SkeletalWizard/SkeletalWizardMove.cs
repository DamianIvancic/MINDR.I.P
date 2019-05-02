using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWizardMove : State<SkeletalWizard>
{ 
    private static SkeletalWizardMove _instance;

    //states this can transit into
    private static SkeletalWizardAggroed _aggroStateReference;
    private static SkeletalWizardDeath _deathStateReference;

    public static SkeletalWizardMove Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalWizardMove();
            }

            return _instance;
        }
    }

    public SkeletalWizardMove()
    {
        if (_instance != null)
            return;
        _instance = this;
        _aggroStateReference = SkeletalWizardAggroed.Instance;
        _deathStateReference = SkeletalWizardDeath.Instance;
    }

    public override void EnterState(SkeletalWizard owner)
    {

        owner.stateFinished = false;
        Debug.Log("State enter: " + this);
    }

    public override void UpdateState(SkeletalWizard owner)
    {
        UpdateAI(owner);
        UpdateMovement(owner);
        if (owner.stateMachine.currentState == this)
            UpdateAnimator(owner);
    }
    
    public override void UpdateAI(SkeletalWizard owner)
    {
        if (owner.isDead)
            owner.stateMachine.ChangeState(_deathStateReference);
        if (owner.aggro)
            owner.stateMachine.ChangeState(_aggroStateReference);


    }

    public override void UpdateAnimator(SkeletalWizard owner)
    {
        if(owner._rb.velocity.magnitude > 0f)
        {
            owner._anim.SetBool("IsWalking", true);
        }
        else
        {
            owner._anim.SetBool("IsWalking", false);
        }
    }

    public override void UpdateMovement(SkeletalWizard owner)
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
                if (owner._isTurnedLeft)
                {
                    Vector2 temp = new Vector2(-1 * owner.speed, owner._gravity);
                    owner._rb.velocity = temp;
                }
                else
                {
                    Vector2 temp = new Vector2(1 * owner.speed, owner._gravity);
                    owner._rb.velocity = temp;
                }
            }
        }
        else
        {
            Debug.Log("I am hit Stunned.");
            Vector2 temp = new Vector2(0, owner._gravity);
            owner._rb.velocity = temp;
        }
    }

    public override void ExitState(SkeletalWizard owner)
    {
        Debug.Log("State exit: " + this);
    }
}
