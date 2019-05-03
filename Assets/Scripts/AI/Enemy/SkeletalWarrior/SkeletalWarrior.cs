using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalWarrior : Enemy {

    [HideInInspector]
    public StateMachine<SkeletalWarrior> stateMachine;

    public LayerMask GroundLayerMask;
    public Transform GroundCheckOrigin;
    public Transform GroundCheckFront;
    public Transform GroundCheckMid;
    
    public float attackCooldown = 3.5f;
    public float attackTimer = 3.5f;
    [HideInInspector]
    public float _gravity = -9.81f; // How fast it gets pulled down
    public bool _isGrounded; // Is it touching the ground?


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<SkeletalWarrior>(this);
        stateMachine.ChangeState(SkeletalWarriorMove.Instance);
        RB = GetComponent<Rigidbody2D>();
    }

    protected override void Update ()
    {
        if (GameManager.GM.CurrentSate == GameManager.GameState.Playing)
        {
            base.Update();

            _isGrounded = (Physics2D.Linecast(GroundCheckOrigin.position, GroundCheckMid.position, GroundLayerMask));

            attackTimer += Time.deltaTime;

            if (aggroLeashTimer < aggroLeashDuration)
            {
                aggro = true;
                aggroLeashTimer += Time.deltaTime;
            }
            else     
                SetAggro(false);
            
            //keep this last in the update
            stateMachine.Update();
        }
    }

    public override void SetAggro(bool state)
    {
        base.SetAggro(state);

        if (state == true)
            aggroLeashTimer = 0f;
    }

    public override bool TakeDamage(int damage = 1)
    {
        SetAggro(true);

        if (hitInvulTimer > hitInvulDuration)
        {
            _damagedSound.Play();
            anim.SetTrigger("Hit");
            HealthManager.Instance.ChargeBerserk();

            hitInvulTimer = 0;

            _currentHP -= damage;
            if (_currentHP <= 0)
                TriggerDeath();

            return true;
        }

        return false;
    }

    public void TurnAround()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1.0f;
        transform.localScale = localScale;
    }
}
