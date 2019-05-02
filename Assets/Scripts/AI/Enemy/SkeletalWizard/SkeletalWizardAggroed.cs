using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWizardAggroed : State<SkeletalWizard>
{

    private static SkeletalWizardAggroed _instance;

    //states this can transit into
    private static SkeletalWizardMove _moveStateReference;
    private static SkeletalWizardDeath _deathStateReference;

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
        _deathStateReference = SkeletalWizardDeath.Instance;
    }

    public override void EnterState(SkeletalWizard owner)
    {      
        Debug.Log("State enter: " + this);
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

    public override void UpdateAI(SkeletalWizard owner)
    {
        if (owner.isDead)
            owner.stateMachine.ChangeState(_deathStateReference);
        if (!owner.aggro)
            owner.stateMachine.ChangeState(_moveStateReference);
    }

    public override void UpdateMovement(SkeletalWizard owner)
    {
        if (!owner.isStunned)
        {
            //Turn to follow player
            if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 0)
            {
                if (owner.transform.localScale.x < 0) owner.TurnAround();
            }
            else
            {
                if (owner.transform.localScale.x > 0) owner.TurnAround();
            }

            //Dont move if player in your range
            if (owner.seesPlayer)
            {
                owner.RB.velocity = new Vector2(0, owner._gravity);
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x > 5.5f)
            {
                Vector2 temp = new Vector2(-1 * owner.speed, owner._gravity);
                owner.RB.velocity = temp;
            }
            else if (owner.transform.position.x - GameManager.GM.Player.transform.position.x < -5.5f)
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

    public override void UpdateAnimator(SkeletalWizard owner)
    {
        if (!owner.isStunned)
        {
            if (owner.seesPlayer)
            {
                if (owner.attackTimer > owner.attackCooldown)
                {
                    owner.anim.SetTrigger("Attack");
                    owner.attackTimer = 0f;
                }
                owner.anim.SetBool("IsWalking", false);
            }
            else if (Mathf.Abs(owner.transform.position.x - GameManager.GM.Player.transform.position.x) > 5.5f)
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

    public override void ExitState(SkeletalWizard owner)
    {
        Debug.Log("State exit: " + this);
    }
}