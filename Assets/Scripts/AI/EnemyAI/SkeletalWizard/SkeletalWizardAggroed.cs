using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWizardAggroed : State<SkeletalWizard> {

    private static SkeletalWizardAggroed _instance;

    //states this can transit into
    private static SkeletalWizardMove _moveStateReference;

    public static SkeletalWizardAggroed Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalWizardAggroed();
            }

            return _instance;
        }
    }

    public SkeletalWizardAggroed()
    {
        if (_instance != null)
            return;
        _instance = this;
        _moveStateReference = SkeletalWizardMove.Instance;
    }

    public override void EnterState(SkeletalWizard owner)
    {
        owner.stateFinished = false;

        Debug.Log("State enter: " + this);
    }

    public override void ExitState(SkeletalWizard owner)
    {
        Debug.Log("State exit: " + this);
    }

    public override void UpdateAI(SkeletalWizard owner)
    {
        if (!owner.aggro)
            owner.stateMachine.ChangeState(_moveStateReference);
    }

    public override void UpdateAnimator(SkeletalWizard owner)
    {
        Debug.Log(owner.transform.position.x - GameManager.GM.Player.transform.position.x);
        if (owner.seesPlayer)
        {
            if (owner.attackTimer > owner.attackCooldown)
            {
                owner._anim.SetTrigger("Attack");
                owner.attackTimer = 0f;
            }
            owner._anim.SetBool("IsWalking", false);
        }
        else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 5.5f)
        {
            owner._anim.SetBool("IsWalking", true);
        }
        else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x < -5.5f)
        {
            owner._anim.SetBool("IsWalking", true);
        }
        
    }

    public override void UpdateMovement(SkeletalWizard owner)
    {
        //Turn to follow player
        if(owner.transform.position.x - GameManager.GM.Player.transform.position.x > 0 )
        {
            if (!owner._isTurnedLeft) owner.TurnAround();
        }
        else
        {
            if (owner._isTurnedLeft) owner.TurnAround();
        }

        //Dont move if player in your range
        if (owner.seesPlayer)
        {
            owner._rb.velocity = new Vector2 (0, owner._gravity);
        }
        else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 5.5f)
        {
            Vector2 temp = new Vector2(-1 * owner.speed, owner._gravity);
            owner._rb.velocity = temp;
        }
        else if(owner.transform.position.x - GameManager.GM.Player.transform.position.x < -5.5f)
        {
            Vector2 temp = new Vector2(1 * owner.speed, owner._gravity);
            owner._rb.velocity = temp;
        }
    }

    public override void UpdateState(SkeletalWizard owner)
    {
        UpdateAI(owner);
        if (owner.stateMachine.currentState == this)
        {
            UpdateMovement(owner);
            UpdateAnimator(owner);
        }
    }
}