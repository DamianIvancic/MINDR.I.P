using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : StateMachineBehaviour
{
    public GameObject DashPoof;
    private Transform _transform;
    private Vector2 _startingPos;
    private Vector2 _scale;
 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {     
        _transform = GameManager.GM.Player.transform;
        _startingPos = _transform.position;
        _scale = _transform.localScale;

        Vector2 PoofPosition = _transform.position;
        PoofPosition.y -= 0.8f;
        if (_scale.x > 0)
            PoofPosition.x -= 0.6f;        
        else          
            PoofPosition.x += 0.6f;
             
        Instantiate(DashPoof, PoofPosition, Quaternion.identity);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Physics2D.Linecast(_transform.position, _scale.x > 0 ? _transform.position + Vector3.right : _transform.position - Vector3.right, GameManager.GM.Player.GroundLayerMask)
            || Physics2D.Linecast(_transform.position, _scale.x > 0 ? _transform.position + new Vector3(1, -1.25f, 0) : _transform.position - new Vector3(1, -1.25f, 0), GameManager.GM.Player.GroundLayerMask))
        {
            animator.SetBool("Dashing", false);
            Debug.Log("Dash collision!");
        }

        if(stateInfo.normalizedTime >= 0.1)
        {
            Vector2 pos = _transform.position;
            pos.x = Mathf.Lerp(_startingPos.x, _scale.x > 0 ? _startingPos.x + 10 : _startingPos.x - 10, stateInfo.normalizedTime - 0.1f);
            _transform.position = pos;
        }
      
        if (stateInfo.normalizedTime >= 0.9)
            animator.SetBool("Dashing", false);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
