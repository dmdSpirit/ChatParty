using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class ViewerBaseController : MonoSingleton<ViewerBaseController> {
	SQLiteConnection connection;

	void Awake () {
		CheckIsSingleInScene ();
		string dbPath = Application.streamingAssetsPath + "/TwitchChatDB.db";
		Logger.Instance.LogMessage("Creating db connection with path: "+dbPath);
		connection = new SQLiteConnection (dbPath, SQLiteOpenFlags.ReadWrite);

	}

	void OnApplicationQuit(){
		Logger.Instance.LogMessage("Closing db connection.");
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
