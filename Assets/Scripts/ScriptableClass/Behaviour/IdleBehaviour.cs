using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Behaviour/IdleBehaviour")]
public class IdleBehaviour : SimpleBehaviourClass {
	public override void Init (BrainController brainController) {
		brainController.Direction =Random.Range (-1, 1) > 0 ? 1 : -1;
		Vector2 idleTime = brainController.GetAvatarStats().idleTime;
		brainController.currentDistance = Random.Range (idleTime.x, idleTime.y);
	}

	public override void Act (BrainController brainController) {
		if (brainController.currentDistance > 0)
			brainController.currentDistance -= Time.deltaTime;
		else
			OnBehaviourEnded (brainController);
	}
}
