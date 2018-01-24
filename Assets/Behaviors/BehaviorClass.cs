using UnityEngine;
using System;

public class BehaviorClass : ScriptableObject {
	public int behaviorWeight = 1;

	public event Action<BehaviorClass> onBehaviorEnded;

	public virtual void Act ( GameObject gameObject){
//		if (onBehaviorEnded != null)
//			onBehaviorEnded ();
	}

}
