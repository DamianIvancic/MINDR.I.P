using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWizardDeath : State<SkeletalWizard> {

    private static SkeletalWizardDeath _instance;

    public static SkeletalWizardDeath Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalWizardDeath();
            }

            return _instance;
        }
    }

    public SkeletalWizardDeath()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void EnterState(SkeletalWizard owner)
    {   
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
    {}

    public override void UpdateMovement(SkeletalWizard owner)
    {
        if (!owner._isGrounded)
        {
            owner.RB.velocity = new Vector2(0, owner._gravity);
        }
        else
        {
            owner.RB.velocity = new Vector2(0, 0);
        }
    }

    public override void UpdateAnimator(SkeletalWizard owner)
    {
        
        owner.anim.SetTrigger("Death");
            
    }

    public override void ExitState(SkeletalWizard owner)
    {
        Debug.Log("State exit: " + this);
    }
}
