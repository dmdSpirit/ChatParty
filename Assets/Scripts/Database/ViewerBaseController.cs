using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class ViewerBaseController : MonoSingleton<ViewerBaseController> {
	SQLiteConnection connection;
	public string viewerName;

	void Awake () {
		CheckIsSingleInScene ();
		Debug.Log("Creating db connection.");
		var dbPath = "Assets/StreamingAssets/TwitchChatDB.db";
		connection = new SQLiteConnection (dbPath, SQLiteOpenFlags.ReadWrite);

	}

	void OnApplicationQuit(){
		connection.Close ();
	}

	public Viewer GetViewer(string name){
		Viewer viewer = connection.Table<Viewer> ().Where (v => v.Name == name).FirstOrDefault ();
		if (viewer == null)
			viewer = CreateViewer (name);
		return viewer;
	}

	Viewer CreateViewer(string name){
		Viewer newViewer = new Viewer{ Name = name, NumberOfMessages = 0, Follower = 0 };
		connection.Insert (newViewer);
		return newViewer;
	}

	public IEnumerable<Viewer> GetAllViewers(){
		return connection.Table<Viewer> ();
	}
}
