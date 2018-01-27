using UnityEngine;

public class WalkBehaviorClass : BehaviorClass {
	public override void Act (AvatarController avatarController) {
		if (avatarController.currentDistance > 0) {
			Vector2 newTransform = avatarController.transform.position;
			float movement = Mathf.Min (avatarController.currentDistance, avatarController.currentSpeed * Time.deltaTime);
			newTransform.x += avatarController.Direction * movement;
			avatarController.transform.position = newTransform;
			avatarController.currentDistance -= movement;
		}

	}

	public override void Init (AvatarController avatarController) {
		Vector2 position = avatarController.transform.position;
		Vector2 travelDistance = avatarController.avatarStats.travelDistance;
		Vector2 speed = avatarController.avatarStats.speed;
		float leftBorder = BackgroundController.Instance.leftBorder.position.x;
		float rightBorder = BackgroundController.Instance.rightBorder.position.x;
		float newDestinationPoin = Mathf.Clamp (position.x + Random.Range (-travelDistance.x, travelDistance.y), 
			leftBorder, rightBorder);
		float newDistance = newDestinationPoin - position.x;
		avatarController.Direction = newDistance > 0 ? 1 : -1;
		avatarController.currentDistance = Mathf.Abs (newDistance);
		avatarController.currentSpeed = Random.Range(speed.x, speed.y);
	}
}
