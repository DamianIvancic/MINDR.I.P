using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatIdle : State<Bat>
{
    //the state this can transition to
    private static BatFlying _flyingReference; 


    private static BatIdle _instance;

    public static BatIdle Instance
    {
        get
        {
            if(_instance == null)
            {
                new BatIdle();
            }

            return _instance;
        }
    }

    public BatIdle()
    {
        if (_instance != null)
            return;

        _instance = this;
        _flyingReference = BatFlying.Instance;
    }
  


    public override void EnterState(Bat owner)
    {}
   
    public override void UpdateState(Bat owner)
    {      
        if (owner.aggro)
            owner.stateMachine.ChangeState(_flyingReference);
    }

    public override void UpdateAI(Bat owner)
    {}

    public override void UpdateMovement(Bat owner)
    {}

    public override void UpdateAnimator(Bat owner)
    {}

    public override void ExitState(Bat owner)
    {}
}
