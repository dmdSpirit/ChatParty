using UnityEngine;

public class MoveBehaviourClass : BehaviourClass {
	[HideInInspector]
	public Vector2 speed;
	[HideInInspector]
	public Vector2 travelDistance;

	public override void Init (AvatarController avatarController) {
		Vector2 position = avatarController.transform.position;
		float leftBorder = BackgroundController.Instance.leftBorder.position.x;
		float rightBorder = BackgroundController.Instance.rightBorder.position.x;
		float newDistance = Random.Range (travelDistance.x, travelDistance.y) * (Random.value > 0.5 ? 1 : -1);
		float newDestinationPoint = position.x + newDistance;
		newDestinationPoint = Mathf.Clamp (newDestinationPoint, leftBorder, rightBorder);
		newDistance = newDestinationPoint - position.x;
		avatarController.Direction = newDistance > 0 ? 1 : -1;
		avatarController.currentDistance = Mathf.Abs (newDistance);
		avatarController.currentSpeed = Random.Range(speed.x, speed.y);
	}

	public override void Act (AvatarController avatarController) {
		if (avatarController.currentDistance > 0) {
			Vector2 newTransform = avatarController.transform.position;
			float movement = Mathf.Min (avatarController.currentDistance, avatarController.currentSpeed * Time.deltaTime);
			newTransform.x += avatarController.Direction * movement;
			avatarController.transform.position = newTransform;
			avatarController.currentDistance -= movement;

		}
		else
			OnBehaviourEnded (avatarController);
	}
}

