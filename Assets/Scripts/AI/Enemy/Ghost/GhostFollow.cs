using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFollow : State<Ghost>
{
    private static GhostIdle _idleReference;
    private static GhostFollow _instance;
    
    public static GhostFollow Instance
    {
        get
        {
            if (_instance == null)
            {
                new GhostFollow();
            }

            return _instance;
        }
    }

    public GhostFollow()
    {
        if (_instance != null)
            return;

        _instance = this;
        _idleReference = GhostIdle.Instance;
    }

    public override void EnterState(Ghost owner)
    {
        owner.speed = 0;
    }

    public override void UpdateState(Ghost owner)
    {  
        UpdateAI(owner);
        UpdateMovement(owner);
    }

    public override void UpdateAI(Ghost owner)
    {
        if (Camera.main.WorldToScreenPoint(owner.transform.position).x < 0
          || Camera.main.WorldToScreenPoint(owner.transform.position).x > Screen.width)
        {
            owner.stateMachine.ChangeState(_idleReference);
        }
    }

    public override void UpdateMovement(Ghost owner)
    {
        Vector3 direction = GameManager.GM.Player.transform.position - owner.transform.position;

        if (direction.magnitude > 0.5f)
        {    
            if (direction.x > 0)
            {
                Vector3 scale = owner.startingScale;
                scale.x *= -1;
                owner.transform.localScale = scale;
            }
            else if (direction.x < 0)
            {
                owner.transform.localScale = owner.startingScale;
            }

            if (direction.x * GameManager.GM.Player.transform.localScale.x > 0) //follow only when the player turns his back
            {
                owner.transform.position += direction.normalized * owner.speed * Time.deltaTime;
                owner.sprite.color = owner.color;
            }
            else
            {
                Color color = owner.color;
                color.a = 0.2f;
                owner.sprite.color = color;
                owner.speed = 0;
            }
        }
        else
        {
            Color color = owner.color;
            color.a = 0.2f;
            owner.sprite.color = color;
            owner.speed = 0;
        }
    }

    public override void UpdateAnimator(Ghost owner)
    {          
    }

    public override void ExitState(Ghost owner)
    { }
}
