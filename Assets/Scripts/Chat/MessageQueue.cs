using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MessageFormatter))]
public class MessageQueue : MonoBehaviour {
	public Text messageText;
	public float messageLifeTime = 8;
	float currentLifeTime = 0;

	MessageFormatter messageFormatter;

	Queue<string> messageQueue;

	void Start(){
		messageFormatter = GetComponent<MessageFormatter> ();
		messageQueue = new Queue<string> ();
	}

	public void AddMessage(string message){
		string[] formatedMessages = messageFormatter.FormatMessage (message).Split('\n');
		int messagesNum = formatedMessages.Length / messageFormatter.maxLines;
		string newMessage;
		for(int i=0; i<messagesNum; i++){
			newMessage = "";
			for (int j = 0; j < messageFormatter.maxLines; j++) {
				if(formatedMessages[i*messageFormatter.maxLines + j] != "")
					newMessage+=formatedMessages[i*messageFormatter.maxLines + j]+"\n";
			}
			newMessage = newMessage.Trim ();
			if (newMessage != "")
				messageQueue.Enqueue (newMessage.TrimEnd ());
		}

	}

	void NextMessage(){
		if (messageQueue.Count > 0)
			messageText.text = messageQueue.Dequeue ();
		else {
			messageText.text = "";
		}
		currentLifeTime = messageLifeTime;
	}

	void Update(){
		currentLifeTime -= Time.deltaTime;
		if(currentLifeTime <= 0)
			NextMessage ();
	}
}
