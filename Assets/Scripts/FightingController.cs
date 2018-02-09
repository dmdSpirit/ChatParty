using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingController : MonoBehaviour {
	public BrainController fighter1;
	public BrainController fighter2;
	public float movementSpeed;

	public void BeginFighting(BrainController fighter1, BrainController fighter2){
		if(fighter1 == null || fighter2 == null){
			Debug.LogError ("FightingController :: fighter missing.");
			return;
		}
		Vector2 newSpeed = (fighter1.GetAvatarStats ().walkSpeed + fighter2.GetAvatarStats ().walkSpeed) / 2;
		movementSpeed = Random.Range (newSpeed.x, newSpeed.y);
		fighter1.InitFightingBrain (this, fighter2);
		fighter2.InitFightingBrain (this, fighter1);

	}

	[ContextMenu("Start fight")]
	public void Test(){
		BeginFighting (fighter1, fighter2);
	}
}
