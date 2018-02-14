using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(MessageQueue))]
[RequireComponent(typeof(AvatarStats))]
public class AvatarController : MonoBehaviour {
	public Text nameText;
	public Viewer viewer;
	public GameObject sprite;
	public AvatarStats avatarStats;

	[HideInInspector]
	public MessageQueue chatMessage;

	Animator spriteAnimator;
	BrainController brainController;



	int direction = 1;
	public int Direction{
		get{ return direction;}
		set{ 
			if (direction != value) {
				sprite.transform.localScale = new Vector2 (value, 1);
				direction = value;
			}
		}
	}

	void Awake(){
		//Debug.Log (gameObject.name +  " :: AvatarController :: Start");
		chatMessage = GetComponent<MessageQueue> ();
		brainController = GetComponent<BrainController> ();
		avatarStats = GetComponent<AvatarStats> ();
	}

	public void SetName (string name){
		nameText.text = name;
	}

	public void SetSprite(string spriteName){
		//Debug.Log (gameObject.name +  " :: AvatarController :: SetSprite");
		GameObject spritePrefab = Resources.Load ("SpritePrefabs/" + spriteName, 
			                       typeof(GameObject)) as GameObject;
		if (spritePrefab != null) {
			sprite = Instantiate (spritePrefab);
			sprite.transform.parent = transform;
		}
		if (sprite != null) {
			spriteAnimator = sprite.GetComponent<Animator> ();
			if(spriteAnimator == null)
				Debug.LogError("AvatarController :: Could not get sprite animator.");
		}
		//Debug.Log ("AvatarController :: SetSprite  -- " + spriteName);
		avatarStats.stats = Resources.Load("AvatarStats/"+spriteName, typeof(AvatarStatsClass)) as AvatarStatsClass;
		//Debug.Log ("AvatarController :: SetSprite  -- " + avatarStats.stats == null);
		sprite.GetComponent<AnimationEventsHandler>().brainController = brainController;
		brainController.InitIdleBrain ();
	}

	public void ChangeAnimation(string trigger){
		if (spriteAnimator != null) {
			spriteAnimator.SetTrigger (trigger);
		}
	}

	public void InitAvatar(Viewer viewer, int sortOrder){
		this.viewer = viewer;
		nameText.text = viewer.Name;
		SetSprite (ViewerBaseController.Instance.GetSpriteName (viewer));
		sprite.GetComponent<SpriteRenderer> ().sortingOrder = sortOrder++;
	}
}
