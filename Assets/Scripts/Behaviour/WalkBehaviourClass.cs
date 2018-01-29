using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Behaviour/WalkingBehaviour")]
public class WalkBehaviourClass : MoveBehaviourClass {
	public override void Init (AvatarController avatarController) {
		speed = avatarController.avatarStats.walkSpeed;
		travelDistance = avatarController.avatarStats.walkTravelDistance;
		base.Init (avatarController);
	}
}
