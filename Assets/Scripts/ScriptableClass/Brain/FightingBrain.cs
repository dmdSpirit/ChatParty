using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Brains/FightingBrain")]
public class FightingBrain : Brain {
	public WalkToBehaviour walkToBehaviour;
	public FightBehaviour fightBehaviour;

	void Awake(){
		if (walkToBehaviour != null)
			walkToBehaviour.BehaviourEnded += OnFinishedWalking;
		if (fightBehaviour != null)
			fightBehaviour.BehaviourEnded += OnFinishedFighting;
	}

	void OnValidate(){
		if (walkToBehaviour != null)
			walkToBehaviour.BehaviourEnded += OnFinishedWalking;
		if (fightBehaviour != null)
			fightBehaviour.BehaviourEnded += OnFinishedFighting;
	}

	public override BehaviourClass InitBrain (BrainController brainController) {
		return walkToBehaviour;
	}

	public void OnFinishedWalking(BrainController brainController){
		brainController.ChangeBehaviour (fightBehaviour);
		Debug.Log (brainController.gameObject.name + " has finished walking to combat line.");

	}

	public void OnFinishedFighting (BrainController brainController){
		Debug.Log (brainController.gameObject.name + " has finished fighting.");
	}
}
