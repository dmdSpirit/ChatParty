using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brain : ScriptableObject {
	//public abstract void Act ();
	public abstract BehaviourClass InitBrain(BrainController brainController);

	public void Act(BrainController brainController){
		brainController.currentBehaviour.Act (brainController);
	}

	public string GetAnimationTrigger (BrainController brainController){
		return brainController.currentBehaviour.animationTrigger;
	}
}
