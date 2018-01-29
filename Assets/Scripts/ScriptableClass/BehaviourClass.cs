using UnityEngine;
using System;

public abstract class BehaviourClass : ScriptableObject {
	public event Action<AvatarController> BehaviourEnded; 

	public int weight;
	public string animationTrigger;

	public virtual void OnBehaviourEnded(AvatarController avatarController){
		if (BehaviourEnded != null)
			BehaviourEnded (avatarController);
	}

	public abstract void Act (AvatarController avatarController);
	public abstract void Init(AvatarController avatarController);
}
