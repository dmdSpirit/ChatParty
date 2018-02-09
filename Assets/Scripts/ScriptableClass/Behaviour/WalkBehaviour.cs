using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Behaviour/WalkingBehaviour")]
public class WalkBehaviour : MoveBehaviour {
	public override void Init (BrainController brainController) {
		speed = brainController.GetAvatarStats().walkSpeed;
		travelDistance = brainController.GetAvatarStats().walkTravelDistance;
		base.Init (brainController);
	}
}
