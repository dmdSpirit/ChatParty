using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AvatarController))]
[RequireComponent(typeof(AvatarStats))]
public class BrainController : MonoBehaviour {
	public IdleBrain idleBrain;
	public float currentSpeed;
	public float currentDistance;

	public FightingBrain fightingBrain;
	public FightingController fightingController;
	public BrainController enemyBrainController;
	public float attackCooldown;

	Brain currentBrain;
	public BehaviourClass currentBehaviour;


	public int Direction{
		get{ return avatarController.Direction;}
		set{ avatarController.Direction = value;}
	}
	AvatarController avatarController;
	AvatarStats avatarStats;

	void Awake(){
		avatarController = GetComponent<AvatarController> ();
		avatarStats = GetComponent<AvatarStats> ();
	}

	void Update(){
		if (currentBrain != null)
			currentBrain.Act (this);
	}

	public AvatarStatsClass GetAvatarStats(){
		return avatarController.avatarStats.stats;
	}

	public void InitIdleBrain(){
		currentBehaviour = idleBrain.InitBrain (this);
		currentBrain = idleBrain;
		InitBehaviour ();
	}

	public void InitFightingBrain(FightingController fController, BrainController enemyBrainController){
		fightingController = fController;
		currentBehaviour = fightingBrain.InitBrain (this);
		currentBrain = fightingBrain;
		this.enemyBrainController = enemyBrainController;
		InitBehaviour ();
	}
		
	public void ChangeBehaviour (BehaviourClass newBehaviour){
		currentBehaviour = newBehaviour;
		InitBehaviour ();
	}

	public void InitBehaviour(){
		currentBehaviour.Init (this);
		ChangeAnimation (currentBrain.GetAnimationTrigger(this));
	}

	public void ChangeAnimation(string animationTrigger){
		avatarController.ChangeAnimation (animationTrigger);
	}

	public void HitEnemy(string hitAnimationTrigger){
		Vector2 damageRange = GetAvatarStats ().damage;
		bool stun = Random.value <= GetAvatarStats ().critChance;
		float critM = stun ? GetAvatarStats ().critMultiplayer : 1;
		float damage = critM * Random.Range(damageRange.x, damageRange.y);
		enemyBrainController.TakeDamage ((int)damage, stun);
		ChangeAnimation (hitAnimationTrigger);
	}

	public void TakeDamage(int damage, bool stun){
		if (stun) {
			ChangeAnimation (fightingBrain.fightBehaviour.hurtAnimationTrigger);
			attackCooldown = GetAvatarStats ().attackCooldown;
		}
		Debug.Log (gameObject.name + " lost " + damage + " HP.");

	}

	public void EndFight(bool win){
		currentBrain = idleBrain;

		InitBehaviour ();
	}
}
