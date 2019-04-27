using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControl;
using System;

/// <summary>
/// Add this a behavior to the base layer of any Animator assigned to a UIPanel to allow the correct
/// functionality of the UI system, since it needs to be notified when the animations start/end
/// to enable/disable the interaction during transitions.
/// </summary>
public class UIPanelAnimationEvents : StateMachineBehaviour {
    
    /// <summary>
    /// flag to indicate if we already notified the end of the animation or not
    /// </summary>
    private bool _ended = false;

    /// <summary>
    /// delegate that will be called when we start the animation
    /// </summary>
    public delegate void OnAnimationStartedDelegate();
    public OnAnimationEndedDelegate OnAnimationStarted;

    /// <summary>
    /// delegate that will be called when the animation ends
    /// </summary>
    public delegate void OnAnimationEndedDelegate();
    public OnAnimationEndedDelegate OnAnimationEnded;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _ended = false;

        //when we start a new animation state it means we are starting a new animation,
        //so call the delegate
        if (OnAnimationStarted != null)
        {
            OnAnimationStarted.Invoke();
        }
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //We check if the animation finished 
        //If it finished and we didnt detect the end of the animation before we call the event to notify 
        //of the end of the animation
        if (stateInfo.normalizedTime >= 1.0f && !_ended)
        {
            if(OnAnimationEnded != null)
            {
                //Debug.LogWarning("INVOKING END ANIMATION");
                OnAnimationEnded.Invoke();
            }

            //we set the flag to true to indicate we already sent the animation end notification and
            //not to do it twice
            _ended = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //sometimes for a 1 frame animation the OnStateUpdate doesnt get detected, so in this method we just make sure
        //that we we exit the animation state if we didnt notify of the animation end we will do it now
        if (!_ended)
        {
            if (OnAnimationEnded != null)
            {
                OnAnimationEnded.Invoke();
            }

            // Debug.LogWarning("We are ending the animation and we should enable interaction " + animator.gameObject.name);
            _ended = true;
        }
        return;
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
