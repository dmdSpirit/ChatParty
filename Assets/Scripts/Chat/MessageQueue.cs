using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component for displaying formated messages from chat.
/// </summary>
public class MessageQueue : MonoBehaviour {
	public Text messageText;
	public float messageLifeTime = 8;

	float currentLifeTime = 0;
	Queue<string> messageQueue = new Queue<string> ();

	void Update(){
		currentLifeTime -= Time.deltaTime;
		if((messageQueue.Count>0 && messageText.text == "") || currentLifeTime <= 0 )
			NextMessage ();
	}

	/// <summary>
	/// Add message to queue.
	/// </summary>
	/// <param name="message">Message.</param>
	public void AddMessage(string message){
		string[] formatedMessages = MessageFormatter.Instance.FormatMessage (message).Split('\n');
		int messagesNum = formatedMessages.Length / MessageFormatter.Instance.maxLines;
		string newMessage;
		for(int i=0; i<messagesNum; i++){
			newMessage = "";
			for (int j = 0; j < MessageFormatter.Instance.maxLines; j++) {
				if(formatedMessages[i*MessageFormatter.Instance.maxLines + j] != "")
					newMessage+=formatedMessages[i*MessageFormatter.Instance.maxLines + j]+"\n";
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
}
