using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIControl
{
    /// <summary>
    /// A class in charge of grouping different panels together. It is used as a way to open
    /// a UIState with all the desired UIPanels at once.
    /// </summary>
    public class UILayout : MonoBehaviour
    {
        /// <summary>
        /// The ID for the layout
        /// </summary>
        [SerializeField]
        UILayoutsIDs _layoutID;
        public UILayoutsIDs GetLayoutID
        {
            get { return _layoutID; }
        }

        /// <summary>
        /// A list of the panels grouped in this layout
        /// </summary>
        [SerializeField]
        protected UIPanelsIDs[] _UIPanelsIDs;
        public UIPanelsIDs[] GetPanelsList()
        {
            return _UIPanelsIDs;
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }


        /// <summary>
        /// It closes the layout, meaning that it closes all the panels assign to it
        /// </summary>
        virtual public void Open()
        {
            IsActive = true;

            foreach (var panelID in _UIPanelsIDs)
            {
                ReferencesHolder.Instance.UIStateManager.OpenPanel(panelID);
            }

        }

        /// <summary>
        /// It opens the layout, meaning that it opens all the panels assign to it
        /// </summary>
        public virtual void Close()
        {
            IsActive = false;

            foreach (var panelID in _UIPanelsIDs)
            {
                ReferencesHolder.Instance.UIStateManager.ClosePanel(panelID);
            }
        }
    }
}