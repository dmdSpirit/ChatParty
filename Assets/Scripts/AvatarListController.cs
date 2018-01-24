using UnityEngine;
using System;
using System.Collections.Generic;

public class AvatarListController : MonoSingleton<AvatarListController> {

	Dictionary<string, AvatarController> viewersDictionary;
	List<string> joinedViewersList;
	List<string> viewersLeftList;
	List<Tuple<string, string>> messageList;

	public GameObject avatarPrefab;

	public string name;
	
	void Start(){
		CheckIsSingleInScene ();
		viewersDictionary = new Dictionary<string, AvatarController> ();
		joinedViewersList = new List<string> ();
		viewersLeftList = new List<string> ();
		messageList = new List<Tuple<string, string>> ();

		//TwitchChat.Instance.onViewerJoined += AddJoinedViewer;
		//TwitchChat.Instance.onViewerLeft += RemoveViewer;
		//TwitchChat.Instance.onNewMessage += AddMessage;


		// FIXME: For testing only.
		AddTestUsers();
	}

	void Update(){
		UpdateViewers ();
	}

	public void AddJoinedViewer(string viewerName){
		Debug.Log ("User joined: " + viewerName);
		joinedViewersList.Add (viewerName);

	}

	public void RemoveViewer(string name){
		viewersLeftList.Add (name);
	}

	public void AddMessage (string name, string message){
		messageList.Add (new Tuple<string, string> (name, message));
	}

	 void UpdateViewers(){
		Viewer viewer;
		GameObject avatar;

		for(int i = joinedViewersList.Count-1; i>=0;i--){
			viewer = ViewerBaseController.Instance.GetViewer (joinedViewersList [i]);
			avatar = Instantiate (avatarPrefab);
			avatar.name = viewer.Name;
			AvatarController avatarController = avatar.GetComponent<AvatarController> ();
			avatarController.SetName (viewer.Name);
			avatarController.viewer = viewer;
			viewersDictionary.Add (viewer.Name, avatarController);
			joinedViewersList.Remove (joinedViewersList [i]);
		}

		for(int i = viewersLeftList.Count -1; i>=0; i--){
			if (viewersDictionary.ContainsKey (viewersLeftList[i])) {
				Destroy (viewersDictionary [viewersLeftList[i]]);
				viewersDictionary.Remove (viewersLeftList[i]);
			} else {
				Debug.LogError ("AvatarListController :: trying to remove user "
					+ viewersLeftList[i] + " that does not exist.");
				foreach (var v in viewersDictionary)
					Debug.Log (v.ToString ());
			}
			viewersLeftList.Remove (viewersLeftList [i]);
		}

		for(int i = messageList.Count - 1; i>= 0; i--){
			Debug.Log ("AvatarListController :: "+ messageList [i].Item1 + " says: " + messageList [i].Item2);
			if(viewersDictionary.ContainsKey(messageList[i].Item1)){
				ChatMessage cm = viewersDictionary [messageList [i].Item1].chatMessage;
				if (cm != null)
					cm.AddMessage (messageList [i].Item2);
				else
					Debug.LogError ("ChatMessage is null");
			}
			messageList.Remove (messageList [i]);
		}
	}

	public void AddTestUsers(){
		GameObject avatar;
		foreach(var viewer in ViewerBaseController.Instance.GetAllViewers()){
			avatar = Instantiate (avatarPrefab);
			avatar.name = viewer.Name;
			AvatarController avatarController = avatar.GetComponent<AvatarController> ();
			avatarController.SetName (viewer.Name);
			avatarController.viewer = viewer;
			viewersDictionary.Add (viewer.Name, avatarController);
		}
	}

}
