using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatMessage))]
public class AvatarController : MonoBehaviour {
	public Text nameText;
	public Viewer viewer;
	public GameObject sprite;
	public AvatarStatsClass avatarStats;
	public Transform spriteTransform;

	int direction = 1;
	public int Direction{
		get{ return direction;}
		set{ 
			if (direction != value) {
				spriteTransform.localScale = new Vector2 (value, 1);
				direction = value;
			}
		}
	}

	public float currentSpeed;
	public float currentDistance;

	[HideInInspector]
	public ChatMessage chatMessage;

	public void Start(){
		chatMessage = GetComponent<ChatMessage> ();
	}

	public void SetName (string name){
		nameText.text = name;
	}


}
