using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : StateMachineBehaviour {

    public GameObject StompHit;
    private Transform _transform;
    private Vector2 _scale;
    private bool _instantiated;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = GameManager.GM.Player.transform;
        _scale = _transform.localScale;

        _instantiated = false;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.8 && !_instantiated)
        {
            Vector2 StompPosition = _transform.position;
            StompPosition.y -= 0.9f;
            if (_scale.x > 0)
                StompPosition.x += 0.35f;
            else
                StompPosition.x -= 0.35f;
            Instantiate(StompHit, StompPosition, Quaternion.identity);
            _instantiated = true;
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
