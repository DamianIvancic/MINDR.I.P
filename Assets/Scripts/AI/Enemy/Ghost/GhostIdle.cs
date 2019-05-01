using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostIdle : State<Ghost>
{
    private static GhostFollow _followReference;
    private static GhostIdle _instance;

    public static GhostIdle Instance
    {
        get
        {
            if (_instance == null)
            {
                new GhostIdle();
            }

            return _instance;
        }
    }

    public GhostIdle()
    {
        if (_instance != null)
            return;

        _instance = this;
        _followReference = GhostFollow.Instance;
    }


    public override void EnterState(Ghost owner)
    {
        Color color = owner.color;
        color.a = 0.2f;
        owner.sprite.color = color;

        owner.aggroZone.SetActive(true);
    }

    public override void UpdateState(Ghost owner)
    {   
        if (owner.aggro)
            owner.stateMachine.ChangeState(_followReference); 
    }

    public override void UpdateAI(Ghost owner)
    {}

    public override void UpdateMovement(Ghost owner)
    {
    }

    public override void UpdateAnimator(Ghost owner)
    { }

    public override void ExitState(Ghost owner)
    { }
}
