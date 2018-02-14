using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "ScriptableObject/Brains/FightBrain")]
public class FightBrain : Brain {
	//public event Action<FightBrain, NextAttack> OnTurnEnded;

	public BehaviourClass moveToBehaviour;
	public AttackClass[] attacks;
	public BehaviourClass idleBehaviour;
	public BehaviourClass looseBehaviour;
	public BehaviourClass winBehaviour;
	// FIXME: Move to behaviour.
	public string hurtTrigger;


	public override BehaviourClass InitBrain (BrainController brainController) {
		throw new System.NotImplementedException ();
	}

	public void StartTurn(BrainController brainController){
		AttackClass currentAttack = ChooseAttack ();
		if (currentAttack != null)
			currentAttack.Attack (brainController);
		else
			Debug.LogError ("FightBrain :: currentAttack is null.");
	}

	public AttackClass ChooseAttack(){
		// TODO: Choose attack from array.
		return attacks [0];
		//return null;
	}
}
