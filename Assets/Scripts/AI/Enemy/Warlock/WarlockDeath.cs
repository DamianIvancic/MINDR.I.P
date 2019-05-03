using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockDeath : State<Warlock>
{
    private static WarlockDeath _instance;

    public static WarlockDeath Instance
    {
        get
        {
            if (_instance == null)
            {
                new WarlockDeath();
            }

            return _instance;
        }
    }

    public WarlockDeath()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void EnterState(Warlock owner)
    {
        owner.anim.SetBool("Death", true);
    }

    public override void UpdateState(Warlock owner)
    { }

    public override void UpdateAI(Warlock owner)
    { }

    public override void UpdateMovement(Warlock owner)
    { }

    public override void UpdateAnimator(Warlock owner)
    { }

    public override void ExitState(Warlock owner)
    { }
}
