using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Attacks/SimpleAttack")]
public class SimpleAttack : AttackClass {
	public override void Attack (BrainController brainController) {
		AvatarStatsClass avatarStats = brainController.GetAvatarStats ();
		Vector2 damageRange = avatarStats.damage;
		bool stun = Random.value <= avatarStats.critChance;
		float critM = stun ? avatarStats.critMultiplayer : 1;
		brainController.damage = critM * Random.Range(damageRange.x, damageRange.y);
		brainController.ChangeAnimation (animationTrigger);
		brainController.turnTimer = cooldown;
	}
}
