using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Behaviour/RuningBehaviour")]
public class RunBehaviour : MoveBehaviour {
	public override void Init (BrainController brainController) {
		speed = brainController.GetAvatarStats().runSpeed;
		travelDistance = brainController.GetAvatarStats().runTravelDistance;
		base.Init (brainController);
	}
}