using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Behaviour/FightBehaviour")]
public class FightBehaviour : BehaviourClass {
	public string attackAnimationTrigger;
	public string hurtAnimationTrigger;
	public string idleAnimationTrigger;

	public override void Init (BrainController brainController) {
		brainController.attackCooldown = (brainController.GetAvatarStats ().attackCooldown + 1) * Random.value;
		animationTrigger = idleAnimationTrigger;
	}

	public override void Act (BrainController brainController) {
		brainController.attackCooldown -= Time.deltaTime;
		if(brainController.attackCooldown <= 0){
			//brainController.HitEnemy (attackAnimationTrigger);
			brainController.attackCooldown = brainController.GetAvatarStats ().attackCooldown;
		}
	}
}
