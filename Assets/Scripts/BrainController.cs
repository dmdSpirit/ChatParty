using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct NextAttack{
	public string attackName;
	public float nextTurnTimer;
}

[RequireComponent(typeof(AvatarController))]
[RequireComponent(typeof(AvatarStats))]
public class BrainController : MonoBehaviour {
	public IdleBrain idleBrain;
	public float currentSpeed;
	public float currentDistance;

	//public old_FightingBrain fightingBrain;
	//public old_FightingController fightingController;
	public FightBrain fightBrain;
	public BrainController enemyBrainController;
	public float attackCooldown;
	NextAttack nextAttack;
	public float initiative;
	public float turnTimer;
	public event Action<BrainController> OnTurnEnded;
	public float damage;
	public int teamID;
	public bool turn = false;

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
		if (currentBrain.GetType() == typeof(IdleBrain) && currentBehaviour != null)
			currentBrain.Act (this);
		if(turn && currentBrain.GetType() == typeof(FightBrain)){
			turn = false;
			((FightBrain)currentBrain).StartTurn (this);
		}
	}

	public AvatarStatsClass GetAvatarStats(){
		return avatarController.avatarStats.stats;
	}

	public void InitIdleBrain(){
		currentBehaviour = idleBrain.InitBrain (this);
		currentBrain = idleBrain;
		InitBehaviour ();
	}

	/*public void InitFightingBrain(old_FightingController fController, BrainController enemyBrainController){
		fightingController = fController;
		currentBehaviour = fightingBrain.InitBrain (this);
		currentBrain = fightingBrain;
		this.enemyBrainController = enemyBrainController;
		InitBehaviour ();
	}*/
		
	public void ChangeBehaviour (BehaviourClass newBehaviour){
		currentBehaviour = newBehaviour;
		InitBehaviour ();
	}

	public void InitBehaviour(){
		currentBehaviour.Init (this);
		ChangeAnimation (currentBrain.GetAnimationTrigger(this));
	}

	public void ChangeAnimation(string animationTrigger){
		Debug.Log (gameObject.name + " animation trigger set to " + animationTrigger);
		avatarController.ChangeAnimation (animationTrigger);
	}

	/*public void HitEnemy(string hitAnimationTrigger){
		Vector2 damageRange = GetAvatarStats ().damage;
		bool stun = UnityEngine.Random.value <= GetAvatarStats ().critChance;
		float critM = stun ? GetAvatarStats ().critMultiplayer : 1;
		float damage = critM * UnityEngine.Random.Range(damageRange.x, damageRange.y);
		enemyBrainController.TakeDamage ((int)damage, stun);
		ChangeAnimation (hitAnimationTrigger);
	}*/

	public void TakeDamage(float damage){
		ChangeAnimation (fightBrain.hurtTrigger);
		attackCooldown = GetAvatarStats ().attackCooldown;
		Debug.Log (gameObject.name + " lost " + damage + " HP.");

	}

	public void EndFight(bool win){
		currentBrain = idleBrain;

		InitBehaviour ();
	}

	public float GetInitiative(){
		initiative = UnityEngine.Random.value;
		return initiative;
	}

	public void TakeTurn(){
		turn = true;
		//TurnEnded ();
	}

	public void TurnEnded(){
		// TODO: turnTimer mast already be calculated.
		if (OnTurnEnded != null)
			OnTurnEnded (this);
	}

	public void InitFightingBrain(){
		currentBrain = fightBrain;
		//currentBehaviour = fightBrain.idleBehaviour;
		currentBehaviour = null;
		//InitBehaviour();
		// FIXME: Get animation trigger from brain.
		ChangeAnimation("Idle");
	}

	public void DealDamage(){
		enemyBrainController.TakeDamage (damage);
	}
}
