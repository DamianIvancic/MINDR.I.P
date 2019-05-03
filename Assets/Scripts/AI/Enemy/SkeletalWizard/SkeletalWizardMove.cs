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

 
    public override void UpdateMovement(SkeletalWizard owner)
    {
        Vector2 temp = new Vector2(0, owner._gravity);
        owner.RB.velocity = temp;


        if (!owner.isStunned)
        {
            if ((owner.transform.position.x - GameManager.GM.Player.transform.position.x) > 0)
                owner.transform.localScale = owner.startingScale;
            else
                owner.transform.localScale = owner.startingScale * -1;
        }
    }

    public override void UpdateAnimator(SkeletalWizard owner)
    {
 
    }

    public override void ExitState(SkeletalWizard owner)
    {
        Debug.Log("State exit: " + this);
    }
}
