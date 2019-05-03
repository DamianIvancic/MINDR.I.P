using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockRetreat : State<Warlock>
{
    private static WarlockAggroed _aggroedStateReference;
    private static WarlockDeath _deathStateReference;

    private static WarlockRetreat _instance;

    public static WarlockRetreat Instance
    {
        get
        {
            if (_instance == null)
            {
                new WarlockRetreat();
            }

            return _instance;
        }
    }

    public WarlockRetreat()
    {
        if (_instance != null)
            return;
        _instance = this;

        _aggroedStateReference = WarlockAggroed.Instance;
        _deathStateReference = WarlockDeath.Instance;
    }

    public override void EnterState(Warlock owner)
    {
        owner.invulnerable = true;

        float distanceA = Mathf.Abs(owner.transform.position.x - owner.RetreatA.position.x);
        float distanceB = Mathf.Abs(owner.transform.position.x - owner.RetreatB.position.x);

        if (distanceA > distanceB)
            owner.targetPos = owner.RetreatA.position;
        else
            owner.targetPos = owner.RetreatB.position;
    }

    public override void UpdateState(Warlock owner)
    {
        Debug.Log("warlock retreat");
        UpdateAI(owner);
        UpdateMovement(owner);
        UpdateAnimator(owner); 
    }

    public override void UpdateAI(Warlock owner)
    {
        if (owner.isDead)
            owner.stateMachine.ChangeState(_deathStateReference);

        if (Mathf.Abs(owner.transform.position.x - owner.targetPos.x) < 1f)
            owner.stateMachine.ChangeState(_aggroedStateReference);
    }

    public override void UpdateMovement(Warlock owner)
    {
        Vector3 targetPos = new Vector3(owner.targetPos.x, owner.transform.position.y, owner.transform.position.z);
        Vector3 direction = targetPos - owner.transform.position;
        direction.Normalize();

        Vector3 scale = owner.startingScale;
        if (direction.x > 0)  
            scale.x *= -1;

        owner.transform.localScale = scale;
       
        owner.transform.position += direction * owner.speed*4 * Time.deltaTime;
    }

    public override void UpdateAnimator(Warlock owner)
    {
        owner.anim.SetBool("Walking", true);
    }

    public override void ExitState(Warlock owner)
    { }
}
