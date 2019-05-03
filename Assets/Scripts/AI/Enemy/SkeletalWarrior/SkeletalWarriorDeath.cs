using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWarriorDeath : State<SkeletalWarrior>
{

    private static SkeletalWarriorDeath _instance;



    public static SkeletalWarriorDeath Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalWarriorDeath();
            }

            return _instance;
        }
    }

    public SkeletalWarriorDeath()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void EnterState(SkeletalWarrior owner)
    {
        MonoBehaviour.Destroy(owner.gameObject, 3f);    
    }

    public override void UpdateState(SkeletalWarrior owner)
    {
        UpdateAI(owner);
        UpdateMovement(owner);
        if (owner.stateMachine.currentState == this)
            UpdateAnimator(owner);
    }

    public override void UpdateAI(SkeletalWarrior owner)
    { }

    public override void UpdateMovement(SkeletalWarrior owner)
    {
        if (!owner._isGrounded)      
            owner.RB.velocity = new Vector2(0, owner._gravity);    
        else      
            owner.RB.velocity = new Vector2(0, 0);     
    }

    public override void UpdateAnimator(SkeletalWarrior owner)
    {

        owner.anim.SetTrigger("Death");

    }

    public override void ExitState(SkeletalWarrior owner)
    {
        Debug.Log("State exit: " + this);
    }
}