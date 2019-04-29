using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWizardMove : State<SkeletalWizard>
{ 
    private static SkeletalWizardMove _instance;

    //states this can transit into
    private static SkeletalWizardAggroed _aggroStateReference;

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
        if (owner.aggro)
            owner.stateMachine.ChangeState(_aggroStateReference);


    }

    public override void UpdateAnimator(SkeletalWizard owner)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateMovement(SkeletalWizard owner)
    {
        if (!Physics2D.Linecast(owner.GroundCheckOrigin.position, owner.GroundCheckFront.position, owner.GroundLayerMask) && owner._isGrounded)
        {
            Debug.Log("I will turn around now.");
            Vector3 localScale = owner._transform.localScale;
            localScale.x *= -1.0f;
            owner._transform.localScale = localScale;

            if (owner._isTurnedLeft) owner._isTurnedLeft = false;
            else owner._isTurnedLeft = true;

        }
        else
        {
            Debug.Log("I can walk forward.");
            if(owner._isTurnedLeft)
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

    public override void ExitState(SkeletalWizard owner)
    {
        Debug.Log("State exit: " + this);
    }
}
