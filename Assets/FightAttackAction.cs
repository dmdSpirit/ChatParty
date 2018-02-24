using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dmdspirit.newScript {
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Fight/Attack")]
    public class FightAttackAction : AvatarCombatAction {
        public override void InitAction(BrainController brainController) {
            brainController.animationController.TriggerAnimation(animationTrigger);
            // TODO: Damage dealt trigger on attack animation clip.
        }

        public override bool Act(BrainController brainController) {
            // Check if animation has ended.
            return brainController.animationController.CheckCurrentAnimationEnded();
        }
    }

}
