using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Behaviour/RuningBehaviour")]
public class RunBehaviourClass : MoveBehaviourClass {
	public override void Init (AvatarController avatarController) {
		speed = avatarController.avatarStats.runSpeed;
		travelDistance = avatarController.avatarStats.runTravelDistance;
		base.Init (avatarController);
	}
}