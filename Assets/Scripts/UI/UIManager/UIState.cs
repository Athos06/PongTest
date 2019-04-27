using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UIControl
{
    /// <summary>
    /// A UIState is a collection of UIPanels "active" (not necesarily open, just added to the state) at any current time. 
    /// We can add/remove panels to a UIState and when we change/close the state all of them will be closed at the same time
    /// </summary>
    public class UIState
    {
        /// <summary>
        /// A list of all the panels added to this UIState. 
        /// </summary>
        [SerializeField]
        private List<UIPanel> _addedPanels;

        /// <summary>
        /// Constructor, creates a new UIState without any active panels
        /// </summary>
        public UIState()
        {
            _addedPanels = new List<UIPanel>();
        }

        /// <summary>
        /// Constructor, creates a new UIState with the UIPanels passed as paremeter
        /// as the active panels
        /// </summary>
        /// <param name="panels">list of active panels for this UIState</param>
        public UIState(IEnumerable<UIPanel> panels)
        {
            _addedPanels = panels.ToList();
        }

        /// <summary>
        /// Adds a panel to the active panels of this UIState
        /// </summary>
        /// <param name="panel">UIPanel to add</param>
        public void AddPanel(UIPanel panel)
        {
            _addedPanels.Add(panel);
        }

        /// <summary>
        /// Removes the specific panel from the active panels of this UIState
        /// </summary>
        /// <param name="panel">UIPanel to remove from the active panels</param>
        public void RemovePanel(UIPanel panel)
        {
            //Debug.Log("trying to remove " + panel);
            _addedPanels.Remove(panel);
        }

        /// <summary>
        /// Called to Open the UIState which will open all the active panels associated to it
        /// </summary>
        public void Open()
        {
            foreach (var panel in _addedPanels)
            {
               panel.Open();
            }
        }

        /// <summary>
        /// Called to close the UIState which will close all the panels associated to it
        /// </summary>
        public void Close()
        {
            foreach (var panel in _addedPanels)
            {
//				Debug.Log ("panel " + panel.gameObject.name);
                panel.Close();
            }
        }
    }
}