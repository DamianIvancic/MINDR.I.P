using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFlying : State<Bat>
{
    private static BatFlying _instance;

    public static BatFlying Instance
    {
        get
        {
            if(_instance == null)
            {
                new BatFlying();
            }

            return _instance;
        }
    }

    public BatFlying ()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public override void EnterState(Bat owner)
    {
        owner.anim.SetBool("Flying", true);
        owner.sineTimer = 0f;
    }

    public override void UpdateState(Bat owner)
    {
        UpdateMovement(owner);
    }

    public override void UpdateAI(Bat owner)
    { }

    public override void UpdateMovement(Bat owner)
    {  
        owner.sineTimer += Time.deltaTime;

        Vector3 pos = owner.transform.position;
        pos += (owner.transform.localScale.x > 0 ? Vector3.right*-1 : Vector3.right) * owner.speed * Time.deltaTime; 
        pos.y = (owner.startingPos + owner.transform.up * Mathf.Sin(owner.sineTimer * owner.frequency) * owner.magnitude).y;
        owner.transform.position = pos;
    }

    public override void UpdateAnimator(Bat owner)
    { }

    public override void ExitState(Bat owner)
    { }

}
