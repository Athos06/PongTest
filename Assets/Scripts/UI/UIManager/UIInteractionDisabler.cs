using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIControl
{
    /// <summary>
    /// Class used to disable the UI interaction when there is some UI transition ongoing, preventing
    /// the UI to get to an inconsistent state when we click on some button during a transition
    /// </summary>
    public class UIInteractionDisabler : MonoBehaviour
    {
        /// <summary>
        /// number of disables queued at the current time. Every panel starting an animation disables the interaction
        // (_disablesCount++) and it enables it back when the animation ends (_disablesCount--)
        /// </summary>
        private int _disablesCount;

        private static UIInteractionDisabler _instance;
        public static UIInteractionDisabler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<UIInteractionDisabler>();

                return _instance;
            }
        }


        [SerializeField]
        CanvasGroup _canvasGroup;

        public void Initialize()
        {
            //by default the UI interaction is enabled
            _disablesCount = 0;

            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        /// <summary>
        /// Called usually from some UIPanel when some transition starts to disable the interaction
        /// with the UI so we cannot click on things during transitions and get to an inconsistent state
        /// (can also be called to disable the UI at any moment if we want)
        /// </summary>
        /// <param name="panelInfo">we can pass the panel that called it to get more debug info</param>
        public void DisableUIInteraction(UIPanel panelInfo = null)
        {
            //we increase the counter for each disable from each panel
            _disablesCount++;

            if (panelInfo != null)
            {
                //Debug.Log("-- Panel " + panelInfo + " disabled UI interaction "
                //    + " (total intereaction disables count " + _disablesCount + " )");
            }

            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

        }
        /// <summary>
        /// Called usually from some UIPanel indicate that its transition finished and we could enabled back the UIInteraction.
        /// It doesn't enable interaction automatically, first it checks if other transitions are still active
        /// </summary>
        /// <param name="panelInfo"></param>
        public void EnableUIInteraction(UIPanel panelInfo = null)
        {
            //we decrease the counter for each disable from each panel
            _disablesCount--;

            if(_disablesCount < 0)
            {
                Debug.LogError("The disables count in the UIInteractionDisabler is less than 0, this shouldnt happen, check for errors");
            }

            if(panelInfo != null)
            {
                //Debug.Log("++ Panel " + panelInfo + " enabled UI interaction "
                //    + " (total intereaction disables count " + _disablesCount + " )" );
            }
            
            //when we dont have any disables it means all animations finished and therefore the transition is over and
            //we can enable the interaction again
            if(_disablesCount == 0)
            { 
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
            }
        }

    }
}