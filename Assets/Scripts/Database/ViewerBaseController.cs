using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
//using System;

public class ViewerBaseController : MonoSingleton<ViewerBaseController> {
	SQLiteConnection connection;

	public int maxSpriteId;
	public int goldenArmor;

	void Awake () {
		CheckIsSingleInScene ();
		string dbPath = Application.dataPath;
		dbPath = dbPath.Substring(0, dbPath.LastIndexOf("/")) + "/DataBase/TwitchChatDB.db";
		Logger.Instance.LogMessage("Creating db connection with path: "+dbPath);
		connection = new SQLiteConnection (dbPath, SQLiteOpenFlags.ReadWrite);
	}

	void OnApplicationQuit(){
		Logger.Instance.LogMessage("Closing db connection.");
		if (connection != null)
			connection.Close ();
	}

	public Viewer GetViewer(string name){
		// Search DB for existing viewer.
		Viewer viewer = connection.Table<Viewer> ().Where (v => v.Name == name).FirstOrDefault ();

		if (viewer == null) {
			viewer = CreateViewer (name);
		}
		else{
			if (viewer.SpriteId != goldenArmor) {
				viewer.SpriteId = Random.Range (1, maxSpriteId + 1);
				connection.Update (viewer);
			}
		}
		//Debug.Log (viewer.ToString ());
		return viewer;
	}

	Viewer CreateViewer(string name){
		int newSpriteID = Random.Range(1, maxSpriteId+1);
		Viewer newViewer = new Viewer{ Name = name, NumberOfMessages = 0, Follower = 0, SpriteId = newSpriteID};
		connection.Insert (newViewer);
		return newViewer;
	}

	public IEnumerable<Viewer> GetAllViewers(){
		return connection.Table<Viewer> ();
	}

	public string GetSpriteName(Viewer viewer){
		SpriteDictionary sprite = connection.Table<SpriteDictionary> ().Where (v => v.id == viewer.SpriteId).First ();
		return sprite.name;
	}
}
