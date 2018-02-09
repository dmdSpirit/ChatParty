using UnityEngine;
using System;
using System.Collections.Generic;

public class AvatarListController : MonoSingleton<AvatarListController> {

	Dictionary<string, AvatarController> viewersDictionary;
	List<string> joinedViewersList;
	List<string> viewersLeftList;
	List<Tuple<string, string>> messageList;
	int avatarSortOrder;

	public GameObject avatarPrefab;

	public new string name;
	public int maxTestViewers =5;
	
	void Start(){
		CheckIsSingleInScene ();
		viewersDictionary = new Dictionary<string, AvatarController> ();
		joinedViewersList = new List<string> ();
		viewersLeftList = new List<string> ();
		messageList = new List<Tuple<string, string>> ();

#if UNITY_STANDALONE_WIN
		//TwitchChat.Instance.onViewerJoined += AddJoinedViewer;
		//TwitchChat.Instance.onViewerLeft += RemoveViewer;
		//TwitchChat.Instance.onNewMessage += AddMessage;
#endif
#if UNITY_EDITOR
		// FIXME: For testing only.
		AddTestUsers();

		//TwitchChat.Instance.onViewerJoined += AddJoinedViewer;
		//TwitchChat.Instance.onViewerLeft += RemoveViewer;
		//TwitchChat.Instance.onNewMessage += AddMessage;
#endif
	}

	void Update(){
		UpdateViewers ();
	}

	public void AddJoinedViewer(string viewerName){
		//Debug.Log ("User joined: " + viewerName);
		if(viewerName != "seniorpomidor")
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
			if (viewersDictionary.ContainsKey (joinedViewersList [i]) == false) {
				viewer = ViewerBaseController.Instance.GetViewer (joinedViewersList [i]);
				avatar = Instantiate (avatarPrefab);
				avatar.name = viewer.Name;
				AvatarController avatarController = avatar.GetComponent<AvatarController> ();
				avatarController.InitAvatar (viewer, avatarSortOrder++);
				viewersDictionary.Add (viewer.Name, avatarController);
			}
			joinedViewersList.Remove (joinedViewersList [i]);
		}

		for(int i = messageList.Count - 1; i>= 0; i--){
			//Debug.Log ("AvatarListController :: "+ messageList [i].Item1 + " says: " + messageList [i].Item2);
			if(viewersDictionary.ContainsKey(messageList[i].Item1)){
				MessageQueue cm = viewersDictionary [messageList [i].Item1].chatMessage;
				if (cm != null)
					cm.AddMessage (messageList [i].Item2);
				else
					Logger.Instance.LogMessage ("AvatarListController :: Error! ChatMessage is null");
				messageList.Remove (messageList [i]);
			}
		}

		for(int i = viewersLeftList.Count -1; i>=0; i--){
			if (viewersDictionary.ContainsKey (viewersLeftList[i])) {
				Destroy (viewersDictionary [viewersLeftList[i]].gameObject);
				viewersDictionary.Remove (viewersLeftList[i]);
				viewersLeftList.Remove (viewersLeftList [i]);
			}

		}
	}

	public void AddTestUsers(){
		GameObject avatar;
		foreach(var viewer in ViewerBaseController.Instance.GetAllViewers()){
			ViewerBaseController.Instance.GetViewer (viewer.Name);
			avatar = Instantiate (avatarPrefab);
			avatar.name = viewer.Name;
			AvatarController avatarController = avatar.GetComponent<AvatarController> ();
			avatarController.InitAvatar (viewer, avatarSortOrder++);
			viewersDictionary.Add (viewer.Name, avatarController);
			if (viewersDictionary.Count >= maxTestViewers)
				break;
		}
	}

}
