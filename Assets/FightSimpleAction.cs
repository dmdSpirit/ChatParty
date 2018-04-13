using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dmdSpirit;

[CreateAssetMenu(menuName = "ScriptableObject/Actions/Fight/SimpleAnimation")]
public class FightSimpleAction : AvatarCombatAction {
    public override void InitAction(BrainController brainController) {
        brainController.animationController.TriggerAnimation(animationTrigger);
    }

    public override bool Act(BrainController brainController) {
        var animationEnded = brainController.animationEventHandler.animationEnded;
        var sameAnimationTrigger = brainController.animationEventHandler.animationTrigger == animationTrigger;
        return animationEnded && sameAnimationTrigger;
    }

}
