using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarBehavior : MonoBehaviour {
	public BehaviorClass[] behaviorArray;

	BehaviorClass currentBehavior;
	int totalWeight;

	void Start(){
		foreach(var behavior in behaviorArray){
			behavior.onBehaviorEnded += OnBehaviorEnded;
			totalWeight += behavior.behaviorWeight;
		}
		SetNewBehavior ();
	}

	void Update(){
		if (currentBehavior != null)
			currentBehavior.Act (gameObject);
	}

	public void OnBehaviorEnded(BehaviorClass behavior){
		SetNewBehavior ();
	}

	void SetNewBehavior(){
/*		float t = Random.value * totalWeight;
		int j = 0;
		float a;
		for(int i=0 ;i<behaviorArray.Length;i++){
			if (t >= a && t < a + behaviorArray [i].behaviorWeight) {
				j = i;
				break;
			}
			a += behaviorArray [i].behaviorWeight;
		}
		if(behaviorArray.Length>0)
			currentBehavior = behaviorArray [j];    */
	}
}
