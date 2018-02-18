using UnityEngine;

/// <summary>
/// Handles animation events on Sprite object. Requires brainController to be set in the inspector.
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimationEventsHandler : MonoBehaviour {
	public BrainController brainController;

	public void OnAttack(){
		if(CheckBrainController())
			brainController.DealDamage ();
	}

	public void OnAnimationEnded(){
		if(CheckBrainController())
			brainController.TurnEnded ();
	}

	bool CheckBrainController(){
		if (brainController == null) {
			Debug.LogError (gameObject.name + " :: AnimationEventsHandler -- brainController is not set.");
			return false;
		} else
			return true;
	}
}
