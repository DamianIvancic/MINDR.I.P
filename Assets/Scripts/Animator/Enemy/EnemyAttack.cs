﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : StateMachineBehaviour {

    private Enemy _owner;
    private Animator _playerAnim; 
    private bool _attackDone;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _owner = animator.gameObject.GetComponent<Enemy>();
        _playerAnim = GameManager.GM.Player.anim;
        _attackDone = false;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    if(stateInfo.normalizedTime > 0.5f && _owner.playerInRange && !_attackDone)
        {
            _attackDone = true;

            if (_playerAnim.GetBool("Blocking") && _playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && GameManager.GM.Player.transform.localScale.x * animator.transform.localScale.x > 0)
                return;

            HealthManager.Instance.TakeDamage(_owner.damage);       
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
