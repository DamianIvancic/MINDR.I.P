using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalRogueDeath : State<SkeletalRogue>
{

    private static SkeletalRogueDeath _instance;



    public static SkeletalRogueDeath Instance
    {
        get
        {
            if (_instance == null)
            {
                new SkeletalRogueDeath();
            }

            return _instance;
        }
    }

    public SkeletalRogueDeath()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void EnterState(SkeletalRogue owner)
    {

        owner.stateFinished = false;
        Debug.Log("State enter: " + this);
    }

    public override void ExitState(SkeletalRogue owner)
    {
        Debug.Log("State exit: " + this);
    }

    public override void UpdateAI(SkeletalRogue owner)
    {

    }

    public override void UpdateState(SkeletalRogue owner)
    {
        UpdateAI(owner);
        UpdateMovement(owner);
        if (owner.stateMachine.currentState == this)
            UpdateAnimator(owner);
    }

    public override void UpdateMovement(SkeletalRogue owner)
    {
        if (!owner._isGrounded)
        {
            owner._rb.velocity = new Vector2(0, owner._gravity);
        }
        else
        {
            owner._rb.velocity = new Vector2(0, 0);
        }
    }

    public override void UpdateAnimator(SkeletalRogue owner)
    {

        owner._anim.SetTrigger("Death");

    }
}