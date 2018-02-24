using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component for displaying formated messages from chat.
/// </summary>
[RequireComponent(typeof(AvatarController))]
public class MessageQueue : MonoBehaviour {
	public float messageLifeTime = 8;

    [SerializeField]
	Text messageText;
    [SerializeField]
    Image messagePanelImage;

	float currentLifeTime = 0;
	Queue<string> messageQueue = new Queue<string> ();

    private void Start() {
        if (messageText == null)
            Logger.LogMessage($"{gameObject.name}::MessageQueue -- messageText field is not set.", LogType.Error);
        else
            messageText.text = "";
        if (messagePanelImage == null)
            Logger.LogMessage($"{gameObject.name}::MessageQueue -- messagePanelImage field is not set.", LogType.Error);
        else
            messagePanelImage.enabled = false;
    }

    private void Update(){
		currentLifeTime -= Time.deltaTime;
		if((messageQueue.Count>0 && messageText.text == "") || currentLifeTime <= 0 )
			NextMessage ();
	}

	/// <summary>
	/// Add message to queue.
	/// </summary>
	/// <param name="message">Message.</param>
	public void AddMessage(string message){
		string[] formatedMessages = MessageFormatter.Instance.SplitMessageIntoLines (message);
        int maxLines = MessageFormatter.Instance.maxLines;
        if(maxLines == 0) {
            Logger.LogMessage("MessageFormatter::maxLines is 0.", LogType.Error);
            maxLines = 1;
        }
        int messagesNum = formatedMessages.Length / maxLines;
		string newMessage;
		for(int i=0; i<messagesNum; i++){
			newMessage = "";
			for (int j = 0; j < maxLines; j++) {
				if(formatedMessages[i* maxLines + j] != "")
					newMessage+=formatedMessages[i* maxLines + j]+"\n";
			}
			newMessage = newMessage.Trim ();
			if (newMessage != "")
				messageQueue.Enqueue (newMessage);
		}
	}

	private void NextMessage(){
        if (messageQueue.Count > 0) {
            messageText.text = messageQueue.Dequeue();
            messagePanelImage.enabled = true;
        }
        else {
            messageText.text = "";
            messagePanelImage.enabled = false;
        }
		currentLifeTime = messageLifeTime;
	}
}
