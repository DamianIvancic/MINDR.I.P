using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockIdle : State<Warlock>
{
    private static WarlockIdle _instance;

    //states this can transit into
    private static WarlockAggroed _aggroStateReference;

    public static WarlockIdle Instance
    {
        get
        {
            if (_instance == null)
            {
                new WarlockIdle();
            }

            return _instance;
        }
    }

    public WarlockIdle()
    {
        if (_instance != null)
            return;
        _instance = this;
        _aggroStateReference = WarlockAggroed.Instance;
    }

    public override void EnterState(Warlock owner)
    { }

    public override void UpdateState(Warlock owner)
    {
        UpdateAI(owner);
    }

    public override void UpdateAI(Warlock owner)
    {
        if (owner.aggro)
            owner.stateMachine.ChangeState(_aggroStateReference);
    }

    public override void UpdateMovement(Warlock owner)
    { }

    public override void UpdateAnimator(Warlock owner)
    { }

    public override void ExitState(Warlock owner)
    { }
}
