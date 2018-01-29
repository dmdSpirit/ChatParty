using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatMessage))]
public class AvatarController : MonoBehaviour {
	public Text nameText;
	public Viewer viewer;
	public GameObject sprite;
	public AvatarStatsClass avatarStats;
	public Transform spriteTransform;
	public BehaviourClass[] behaviours;

	public float currentSpeed;
	public float currentDistance;

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

	[HideInInspector]
	public ChatMessage chatMessage;

	int totalBehaviourWeight;
	public BehaviourClass currentBehaviour;
	Animator spriteAnimator;

	void Start(){
		chatMessage = GetComponent<ChatMessage> ();
		if (sprite != null) {
			spriteAnimator = sprite.GetComponent<Animator> ();
			if(spriteAnimator == null)
				Debug.LogError("AvatarController :: Could not get sprite animator.");
		}

		InitBehaviour ();
	}

	void Update(){
		if (currentBehaviour != null)
			currentBehaviour.Act (this);
	}

	public void SetName (string name){
		nameText.text = name;
	}

	public void InitBehaviour(){
		if (behaviours.Length == 0)
			Debug.LogWarning ("AvatarController :: No behaviours assigned to " + gameObject.name);
		foreach (var behaviour in behaviours) {
			behaviour.BehaviourEnded += ChangeBehaviour;
			totalBehaviourWeight += behaviour.weight;
		}
		ChangeBehaviour (this);
	}

	public void ChangeBehaviour(AvatarController avatarController){
		if (avatarController != this)
			return;
		currentBehaviour = GetRandomBehaviour ();
		currentBehaviour.Init (this);
		if(spriteAnimator!=null)
			spriteAnimator.SetTrigger (currentBehaviour.animationTrigger);
	}

	public BehaviourClass GetRandomBehaviour(){
		if (behaviours.Length != 0) {
			float t = Random.Range (0, totalBehaviourWeight);
			int newBehaviour=0;
			int currentWeight=0;
			for (int i = 0; i < behaviours.Length; i++) {
				if (t < currentWeight + behaviours [i].weight) {
					newBehaviour = i;
					break;
				}
				currentWeight += behaviours [i].weight;
			}
			//Debug.Log (gameObject.name + " behaviour changed to " + behaviours [newBehaviour].name);
			return behaviours [newBehaviour];
		}
		else{
			return null;
		}
	}
}
