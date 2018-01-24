using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Behaviors/Walk")]
public class WalkBehavior : BehaviorClass {
	
	public float currentDistance = 0f;
	public float maxDistance = 10f;
	public float currentSpeed;
	public Vector2 speed = new Vector2 ();

	public Transform spriteTransform;

	int direction = 1; // 1 = move right, -1 = move left.
	int Direction{
		get{ return direction;}
		set{ 
			if (direction != value) {
				spriteTransform.localScale = new Vector2 (value, 1);
				direction = value;
			}
		}
	}

	public override void Act (GameObject gameObject) {
		if (currentDistance == 0)
			SetNewDistance (gameObject);
		Vector2 newTransform = gameObject.transform.position;
		float movement = Mathf.Min (currentDistance, currentSpeed * Time.deltaTime) ;
		newTransform.x += direction * movement;
		gameObject.transform.position = newTransform;
		currentDistance -= movement;
//		base.Act ();
	}

	public void SetNewDistance(GameObject gameObject){
		float newDestinationPoin = Mathf.Clamp (gameObject.transform.position.x + Random.Range (-maxDistance, maxDistance),
			BackgroundController.Instance.leftBorder.position.x,
			BackgroundController.Instance.rightBorder.position.x);
		currentDistance = newDestinationPoin - gameObject.transform.position.x;
		Direction = currentDistance > 0 ? 1 : -1;
		currentDistance = Mathf.Abs (currentDistance);
		currentSpeed = Random.Range(speed.x, speed.y);
	}
}
