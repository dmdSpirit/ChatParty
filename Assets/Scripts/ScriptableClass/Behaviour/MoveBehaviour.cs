using UnityEngine;

public class MoveBehaviour : SimpleBehaviourClass {
	[HideInInspector]
	public Vector2 speed;
	[HideInInspector]
	public Vector2 travelDistance;

	public override void Init (BrainController brainController) {
		Vector2 position = brainController.transform.position;
		float leftBorder = BackgroundController.Instance.leftBorder.position.x;
		float rightBorder = BackgroundController.Instance.rightBorder.position.x;
		float newDistance = Random.Range (travelDistance.x, travelDistance.y) * (Random.value > 0.5 ? 1 : -1);
		float newDestinationPoint = position.x + newDistance;
		newDestinationPoint = Mathf.Clamp (newDestinationPoint, leftBorder, rightBorder);
		newDistance = newDestinationPoint - position.x;
		brainController.Direction= newDistance >= 0 ? 1 : -1;
		brainController.currentDistance = Mathf.Abs (newDistance);
		brainController.currentSpeed = Random.Range(speed.x, speed.y);
	}

	public override void Act (BrainController brainController) {
		if (brainController.currentDistance > 0) {
			Vector2 newTransform = brainController.transform.position;
			float movement = Mathf.Min (brainController.currentDistance, brainController.currentSpeed * Time.deltaTime);
			newTransform.x += brainController.Direction * movement;
			brainController.transform.position = newTransform;
			brainController.currentDistance -= movement;

		}
		else
			OnBehaviourEnded (brainController);
	}
}

