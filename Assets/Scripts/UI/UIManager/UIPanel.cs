using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIControl
{
    /// <summary>
    /// Class for all the functionaly of the UIPanel. In charge of the correct functioning of the panel
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class UIPanel : MonoBehaviour
    {
        /// <summary>
        /// enum used as an identificator for the panel (each ID must be unique and can only be used once)
        /// </summary>
        [SerializeField]
        UIPanelsIDs _panelID;
        public UIPanelsIDs GetUIPanelID
        {
            get { return _panelID; }
        }

        /// <summary>
        /// Animator assigned to the UIPanel. the UIPanel can work without a UIPanel
        /// (in that case the "animation" will be just enable/disable the game object)
        /// but it is highly recommendable to use one and it will be added as a component as default.
        /// If you dont want to use it just turn of the Animator component in the game object on the editor
        /// </summary>
        [SerializeField]
        private Animator _animator;

        /// <summary>
        /// flag to indicate if the panel should be open at start or not
        /// </summary>
        [SerializeField]
        private bool _openAtStart;

		//[SerializeField]
		//private string panelTitle;

		private bool _isOpen = false;
        public bool IsActive
        {
            get {
                return _isOpen;
            }
        }

        /// <summary>
        /// gives access to the UIPanelANimationEvents assigned as behaviours to the animator.
        /// It allows us to subscribe to the start/end animation events (Look into OnAnimationStarted()
        /// and OnAnimationEnded() to understand their use)
        /// </summary>
        private UIPanelAnimationEvents[] animationEvents;

     

        public void Initialize()
        {
            _isOpen = _openAtStart;
//			Debug.Log ("IS OPEN" + _openAtStart);
            // In case the gameObject was disabled we always need to activate it first
            gameObject.SetActive(true);

            if (_animator != null)
            {
                //we need to keep a naming convention and always use IsOpen for the name of the parameter in the animator
                _animator.SetBool("IsOpen", _openAtStart);

                animationEvents = _animator.GetBehaviours<UIPanelAnimationEvents>();
                if (animationEvents.Length > 0) {
                    //we subcribe to the events for start/end animation of each animation
                    foreach (var animEvent in animationEvents) { 
                        animEvent.OnAnimationEnded += OnAnimationEnded;
                        animEvent.OnAnimationStarted += OnAnimationStarted;
                    }
                }
                else
                {
                    //If we dont have any behavior assigned to the animator we forgot to assign it on the editor
                    //and we need them
                    Debug.LogError("The animator " + _animator + " in " + name +
                        " has no AnimationEnded behaviour assigned! " +
                        "(the animator needs an AnimationEvents behaviour assigned to work properly)");
                }
            }
            else
            {
                // if we dont have an animator assigned we just enable/disable the gameObject instead of having an animation.
                // At first we dont want this, its better to have an animator, but it also works so we allow it (with a warning 
                // just in case we didnt add it by mistake)
                Debug.LogWarning("Missing animator in panel " + name + " !");
                gameObject.SetActive(_isOpen);
            }
        }

        /// <summary>
        /// Opens the panel
        /// </summary>
        public virtual void Open()
        {
           
            //we shouldnt try to open and already open panel
            if (_isOpen)
                return;

            gameObject.SetActive(true);

            if (_animator != null) { 
                _animator.SetBool("IsOpen", true);
            }
            else
            {
                gameObject.SetActive(true);
            }            

            _isOpen = true;
        }

        /// <summary>
        /// Closes the panel
        /// </summary>
        public virtual void Close()
        {
            //we shouldnt try to close an already closed panel
            if (!_isOpen)
                return;

            _isOpen = false;

            if (_animator != null)
            {
                _animator.SetBool("IsOpen", false);
            }
            else
            {
                //Debug.LogWarning("Missing animator in panel " + name + " !");
                gameObject.SetActive(false);
            }
        }

        private void OnAnimationStarted()
        {
            //when the animation starts iti means a UI transition is happening so 
            //we call the UIInteractionDisabler to disable the interaction during the transition
            ReferencesHolder.Instance.UIStateManager.GetUIInteractionDisabler.DisableUIInteraction(this);
        }

        private void OnAnimationEnded()
        {
            if (!_isOpen) { }
            //When the animation ends we want to enable back the interaction since it means
            //the transition is over
            ReferencesHolder.Instance.UIStateManager.GetUIInteractionDisabler.EnableUIInteraction(this);
        }

    }
}
