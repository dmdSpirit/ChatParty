﻿using UnityEngine;

using System;

using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;

public class TwitchChat : MonoSingleton<TwitchChat> {
	TwitchClient client;
	ConnectionCredentials credentials;

	public event Action<string> onViewerJoined;
	public event Action<string> onViewerLeft;
	public event Action<string, string> onNewMessage;

	bool isConnected = false;

	void Awake(){
		credentials = new ConnectionCredentials (Credentials.userName, Credentials.accessToken);
	}

	void Start () {
		CheckIsSingleInScene ();
		client = new TwitchClient(credentials, Credentials.channelToJoin);

		client.OnMessageReceived += OnMessageReceived;
		client.OnUserJoined += OnUserJoined;
		client.OnUserLeft += OnUserLeft;

		// FIXME: For testing only.
		client.Connect ();
		if(client.IsConnected)
			Debug.Log("Twitch client connected.");

	}

	void Update(){
		if(isConnected == false && client.IsConnected){
			Debug.Log("Twitch client connected.");
			isConnected = true;
		}
			
	}

	void OnApplicationQuit(){
		if (client.IsConnected) {
			client.Disconnect ();
			Debug.Log ("Twitch client disconnected.");
		}
	}

	void OnUserLeft (object sender, OnUserLeftArgs e)
	{
		if (onViewerLeft != null)
			onViewerLeft (e.Username);
	}

	void OnMessageReceived (object sender, OnMessageReceivedArgs e)
	{
		if (onNewMessage != null)
			onNewMessage (e.ChatMessage.Username, e.ChatMessage.Message);
		Debug.Log (e.ChatMessage.DisplayName + " says: " + e.ChatMessage.Message);
	}

	void OnUserJoined (object sender, OnUserJoinedArgs e) {
		Debug.Log ("User joined: " + e.Username);
		if (onViewerJoined != null)
			onViewerJoined (e.Username);
	}

}
