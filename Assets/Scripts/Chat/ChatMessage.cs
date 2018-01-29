using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage : MonoBehaviour {
	public Text messageText;
	public float messageLifeTime = 8;
	float currentLifeTime = 0;

	Queue<string> messageQueue;

	void Start(){
		messageQueue = new Queue<string> ();
	}

	public void AddMessage(string message){
		messageQueue.Enqueue(message);
	}

	void NextMessage(){
		if(messageQueue.Count > 0)
			messageText.text = messageQueue.Dequeue();
		else 
			messageText.text = "";
		currentLifeTime = messageLifeTime;
	}

	void Update(){
		currentLifeTime -= Time.deltaTime;
		if(currentLifeTime <= 0)
			NextMessage ();
	}
}
