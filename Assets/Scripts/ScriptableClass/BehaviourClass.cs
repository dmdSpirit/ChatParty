using UnityEngine;
using System;

public abstract class BehaviourClass : ScriptableObject {
	public event Action<BrainController> BehaviourEnded; 
	public string animationTrigger;

	public virtual void OnBehaviourEnded(BrainController brainController){
		if (BehaviourEnded != null)
			BehaviourEnded (brainController);
	}

	public abstract void Act (BrainController brainController);
	public abstract void Init(BrainController brainController);
}
