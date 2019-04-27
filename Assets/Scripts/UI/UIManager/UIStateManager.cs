using System.Collections.Generic;
using UnityEngine;

namespace UIControl {
	/// <summary>
	/// Class in charge of controlling the state of the UI, offering the functionality to close/open all the different
	/// UILayouts and UIPanels
	/// </summary>
	public class UIStateManager : MonoBehaviour {
		// events //
		public delegate void OpenLayoutAction (UILayoutsIDs id);
		public event OpenLayoutAction OnLayoutOpen;

		public delegate void OpenPanelAction (UIPanelsIDs id);
		public event OpenPanelAction OnPanelOpen;

		[Header ("LAYOUTS")]
		[Tooltip ("Drag and drop all the used UILayouts here to add them and be able to use them")]
		[SerializeField]
		private List<UILayout> _UILayoutsList = new List<UILayout> ();
		[Header ("PANELS")]
		[Tooltip ("Drag and drop all the used UIPanels here to add them and be able to use them")]
		[SerializeField]
		private List<UIPanel> _UIPanelsList = new List<UIPanel> ();

		[Header ("INTERACTION DISABLER")]
		[SerializeField]
		private UIInteractionDisabler _interactionDisabler;
		public UIInteractionDisabler GetUIInteractionDisabler {
			get { return _interactionDisabler; }
		}

		/// <summary>
		/// Dictionary with all the availables UILayouts
		/// </summary>
		private Dictionary<UILayoutsIDs, UILayout> _layoutsDictionary;
		/// <summary>
		/// Dictionary with all the availables UIPanels
		/// </summary>
		private Dictionary<UIPanelsIDs, UIPanel> _panelsDictionary;

		/// <summary>
		/// The stack with the UIstates. When we open a new UIState we stack it here, when we close it we unstack it
		/// </summary>
		private Stack<UIState> _UIStatesStack = new Stack<UIState> ();

		private UILayoutsIDs _currentLayoutOpen;

	/// <summary>
	/// The current UIstate that we are in.
	/// </summary>
	public UIState CurrentState {
			get {
				if (_UIStatesStack.Count == 0)
					return null;

				return _UIStatesStack.Peek ();
			}
		}

		/// <summary>
		/// Lazy singleton
		/// </summary>
		private static UIStateManager _instance = null;
		public static UIStateManager Instance {
			get {
				if (_instance == null)
					_instance = (UIStateManager)FindObjectOfType (typeof (UIStateManager));
				return _instance;
			}
		}

		public void Awake ()
		{
			if (_instance == null) _instance = this;
		}

		bool _initialized = false;
		/// <summary>
		/// Called for initialization. Should be called from the app entry point
		/// </summary>
		public void Initialize ()
		{
			if (_initialized)
				return;

			_initialized = true;

			_interactionDisabler.Initialize ();

			_layoutsDictionary = new Dictionary<UILayoutsIDs, UILayout> ();
			_panelsDictionary = new Dictionary<UIPanelsIDs, UIPanel> ();

			_UIStatesStack.Clear ();

			//We are going to create the panel dictionary going through all the panels in the panels list
			//and adding them with their ID as the ke
			foreach (var panel in _UIPanelsList) {
				//we also initialize each panel first
				panel.Initialize ();

				if (panel.GetUIPanelID == UIPanelsIDs.UNASSIGNED) {
					Debug.LogError ("UNASSIGNED UIPanelID in " + panel.name + " !");
				}

				//We want to check if we made some mistake adding the panels to the panels list.
				//We can only add the panel once, so if we already had the panelID in the dictionary 
				//it means we have either the same panel twice on the list or some different panel with the same 
				//ID by mistake
				if (_panelsDictionary.ContainsKey (panel.GetUIPanelID)) {
					Debug.LogError ("the UIPanel ID " + panel.GetUIPanelID +
					    " has been added twice! in " + _panelsDictionary [panel.GetUIPanelID].name +
					    " and " + panel.name + " . Check in  " + name + " in the UIPanelList to fix the problem");
				} else {
					_panelsDictionary.Add (panel.GetUIPanelID, panel);
				}
			}

			//We do the same but now for layouts
			foreach (var layout in _UILayoutsList) {
				if (layout.GetLayoutID == UILayoutsIDs.UNASSIGNED) {
					Debug.LogError ("UNASSIGNED UILayoutID in " + layout.name + " !");
				}

				if (_layoutsDictionary.ContainsKey (layout.GetLayoutID)) {
					Debug.LogError ("the layout ID  " + layout.GetLayoutID +
					  " has been added twice! in " + _layoutsDictionary [layout.GetLayoutID].name +
					  " and " + layout.name + " . Check in  " + name + " in the UILayoutsList to fix the problem");
				} else {
					_layoutsDictionary.Add (layout.GetLayoutID, layout);
				}
			}

			////And finally we will just open the start layout (login in this case)
			////IMPORTANT TODO this should be done better at the entry point of the app, somewhere else controlling the app flow
			////not here in the UIStateManager though
			//OpenLayout (UILayoutsIDs.DownloadModel);
		}


		/// <summary>
		/// Closes all UIStates and empty the stack. Used when you just want to start with a new UIState
		/// and you dont want any previous one in the stack (use case: when clicking on log out we want
		/// to just start openning the login layout as the only UIState)
		/// </summary>
		public void CloseAll ()
		{
//			Debug.Log ("------Close all " + _UIStatesStack.Count);

			if (_UIStatesStack.Count > 0) {
				for (int i = _UIStatesStack.Count; i > 0; i--) {
					var state = _UIStatesStack.Pop ();

					state.Close ();
				}
			}

			_UIStatesStack.Clear ();
		}

		#region UILayouts methods

		/// <summary>
		/// call to unstack the last added UIState, closing it and opening back the previous state
		/// </summary>
		public void CloseLastState ()
		{
			if (_UIStatesStack.Count > 0) {
				_UIStatesStack.Pop ().Close ();
			}
			if (_UIStatesStack.Count > 0) {
				_UIStatesStack.Peek ().Open ();
			}
		}

		/// <summary>
		/// It finds and opens the indicated layout as a new UIState
		/// </summary>
		/// <param name="layoutID">ID for the layout</param>
		/// <param name="openOnTop">If true it will open the new layout on top of the previous
		/// one without closing it (useful for things like modal dialogs or layouts that actually work together
		/// with the previous one)</param>
		public void OpenLayout (UILayoutsIDs layoutID, bool openOnTop = false)
		{
            //Debug.Log("Open layout " + layoutID);
            _currentLayoutOpen = layoutID;
			List<UIPanel> panels = new List<UIPanel> ();

			foreach (var panelID in FindLayoutByID (layoutID).GetPanelsList ()) {
//				Debug.Log ("Panel ID  " + panelID);
				panels.Add (UIStateManager.Instance.FindPanelByID (panelID));
			}

			UIState newState = new UIState (panels);

			if (openOnTop == false) {
				if (_UIStatesStack.Count > 0)
					_UIStatesStack.Peek ().Close ();
			}

			newState.Open ();
			_UIStatesStack.Push (newState);
			if (OnLayoutOpen != null) {
				OnLayoutOpen (layoutID);
			}

        }

		/// <summary>
		/// Finds the layout in the layouts dictionary by the ID
		/// </summary>
		/// <param name="layoutID">ID to look for</param>
		/// <returns>the found layout</returns>
		public UILayout FindLayoutByID (UILayoutsIDs layoutID)
		{
			if (!_layoutsDictionary.ContainsKey (layoutID)) {
				Debug.LogError ("the layout ID  " + layoutID + " cannot be found in the layoutsDictionary. " +
				    " Check the UIStateManager and add it there if it is missing");
			}

			return _layoutsDictionary [layoutID];
		}

		#region unused UILayouts methods that could be useful in the future
		//UNUSED RIGHT NOW BUT MAYBE USEFUL IN THE FUTURE
		public bool IsUILayoutActive (UILayoutsIDs name)
		{
			return FindLayoutByID (name).IsActive;
		}
		#endregion

		#endregion

		#region UIPanels methods
		/// <summary>
		/// Opens panel found by the ID passed and adds it to the current UIState
		/// </summary>
		/// <param name="name">the panel ID</param>
		public void OpenPanel (UIPanelsIDs name)
		{

			UIPanel panel = FindPanelByID (name);

			panel.Open ();

			//when we use OpenPanel we just want to open the panel in the current state no matter which one it is,
			//this way we can open any panel we need at any moment no matter what
			if (CurrentState != null) {
				CurrentState.AddPanel (panel);
			} else {
				//if there is no UIState active we will just create a new one and we add the panel to it
				//(this option is not used so far, just in case for the future)
				UIState newState = new UIState (new UIPanel [] { panel });
			}
			if (OnPanelOpen != null) {
				OnPanelOpen (name);
			}
		}

		/// <summary>
		/// Closes the panel found by the ID and removes it from any UIState where it was open.
		/// If the panel was open in more than one state (this case doesnt happen now and it would be strange, 
		/// but not imposible) it will be closed in all UIStates on the stack, even if they are not active.
		/// </summary>
		/// <param name="name"></param>
		public void ClosePanel (UIPanelsIDs name)
		{
			UIPanel panel = FindPanelByID (name);
			panel.Close ();

			//we look for the panel in all the UIStates on the stack 
			//and we remove it from them
			foreach (var state in _UIStatesStack) {
				state.RemovePanel (panel);
			}

		}


		/// <summary>
		/// Finds the panel in the panels dictionary by the panelID
		/// </summary>
		/// <param name="panelID">ID to look for</param>
		/// <returns>The found panel</returns>
		public UIPanel FindPanelByID (UIPanelsIDs panelID)
		{
			if (!_panelsDictionary.ContainsKey (panelID)) {
				Debug.LogError ("the panel ID  " + panelID + " cannot be found in the panelsDictionary. " +
				    " Check the UIStateManager and add it there if it is missing");
			}

			return _panelsDictionary [panelID];
		}

		/// <summary>
		/// Opens the panel passed as parameter and adds it to the current UIState
		/// </summary>
		/// <param name="panel">Panel to open</param>
		public void OpenPanel (UIPanel panel)
		{
			panel.Open ();

			if (CurrentState != null) {
				CurrentState.AddPanel (panel);
			} else {
				UIState newState = new UIState (new UIPanel [] { panel });
			}

	    		
		}

		/// <summary>
		/// Closes the panel passed as parameter and removes it from any UIState where it was open.
		/// If the panel was open in more than one state (this case doesnt happen now and it would be strange, 
		/// but not imposible) it will be closed in all UIStates on the stack, even if they are not active.
		/// </summary>
		/// <param name="panel"> Panel to open </param>
		public void ClosePanel (UIPanel panel)
		{
			panel.Close ();

			if (CurrentState != null)
				CurrentState.RemovePanel (panel);
		}

		#region unused panel methods that could be useful in the future


		public bool IsUIPanelActive(UIPanelsIDs name)
		{
		    return FindPanelByID(name).IsActive;
		}
		#endregion

		#endregion

	}
}