using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastFireball : StateMachineBehaviour {

    public GameObject Fireball;

    private Transform _transform;

    public float castTime;
    public bool transformInverted;
    private bool _castCompleted;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.transform;
        _castCompleted = false;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    if(stateInfo.normalizedTime > castTime && _castCompleted == false)
        {
            Vector3 castPosition;
            float xScaleMultiplier;

            if (_transform.localScale.x < 0)
            {
                castPosition = _transform.position + new Vector3(transformInverted ? -1 : 1, 0, 0);
                xScaleMultiplier = transformInverted ? 1 : -1;
            }
            else
            {
                castPosition = _transform.position + new Vector3(transformInverted ? 1 : -1, 0, 0);
                xScaleMultiplier = transformInverted ? -1 : 1;
            }
            
     
            GameObject fireball = Instantiate(Fireball, castPosition, Quaternion.identity);

            Vector3 scale = fireball.transform.localScale;
            scale.x *= xScaleMultiplier;
            fireball.transform.localScale = scale;
            _castCompleted = true;
        }

        if (stateInfo.normalizedTime > 0.9f)
            animator.SetBool("Casting", false);
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
