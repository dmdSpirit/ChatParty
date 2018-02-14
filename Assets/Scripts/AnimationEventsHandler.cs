using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour {
	public BrainController brainController;

	public void OnAttack(){
		brainController.DealDamage ();
	}

	public void OnAnimationEnded(){
		brainController.TurnEnded ();
	}
}
