using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Behaviour/IdleBehaviour")]
public class IdleBehaviourClass : BehaviourClass {
	public override void Init (AvatarController avatarController) {
		avatarController.Direction = Random.Range (-1, 1) > 0 ? 1 : -1;
		Vector2 idleTime = avatarController.avatarStats.idleTime;
		avatarController.currentDistance = Random.Range (idleTime.x, idleTime.y);
	}

	public override void Act (AvatarController avatarController) {
		if (avatarController.currentDistance > 0)
			avatarController.currentDistance -= Time.deltaTime;
		else
			OnBehaviourEnded (avatarController);
	}
}
