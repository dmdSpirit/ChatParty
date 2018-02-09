using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Behaviour/WalkToBehaviour")]
public class WalkToBehaviour : SimpleBehaviourClass {

	public override void Init (BrainController brainController) {
		brainController.currentSpeed = brainController.fightingController.movementSpeed;
		float newDistance = 
			brainController.enemyBrainController.transform.position.x - brainController.transform.position.x;
		brainController.Direction = newDistance >= 0 ? 1 : -1;
		brainController.currentDistance = Mathf.Abs (newDistance);
	}
	
	public override void Act (BrainController brainController) {
		float distanceLeft = 
			brainController.enemyBrainController.transform.position.x - brainController.transform.position.x;
		distanceLeft = Mathf.Abs (distanceLeft);
		if (distanceLeft > brainController.GetAvatarStats().attackDistance) {
			Vector2 newTransform = brainController.transform.position;
			float movement = Mathf.Min (distanceLeft, brainController.currentSpeed * Time.deltaTime);
			newTransform.x += brainController.Direction * movement;
			brainController.transform.position = newTransform;
			brainController.currentDistance -= movement;

		}
		else
			OnBehaviourEnded (brainController);
	}
}
