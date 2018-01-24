using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatMessage))]
[RequireComponent(typeof(AvatarBehavior))]
public class AvatarController : MonoBehaviour {
	public Text nameText;
	public Viewer viewer;

	[HideInInspector]
	public ChatMessage chatMessage;

	public void Start(){
		chatMessage = GetComponent<ChatMessage> ();
	}

	public void SetName (string name){
		nameText.text = name;
	}


}
