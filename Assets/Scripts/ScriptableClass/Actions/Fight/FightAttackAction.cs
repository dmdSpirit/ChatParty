using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Simple attack from Avatar Combat Actions.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Fight/Attack")]
    public class FightAttackAction : AvatarCombatAction {
        // TODO: Check if enemy is close enough or set weight to 0.

        /// <summary>
        /// Initializing function for action.
        /// </summary>
        /// <param name="brainController">BrainController.</param>
        public override void InitAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainController.animationController.TriggerAnimation(animationTrigger);
            float attackDirection = brainController.transform.position.x > brainVariables.currentEnemy.transform.position.x ? -1 : 1;
            brainController.animationController.Direction = attackDirection;
            //Logger.LogMessage($"{brainController.gameObject.name}::FightAttackAction::InitAction;");

            // TODO: Damage dealt trigger on attack animation clip.
            // TODO: If attacking from behind => bonus damage?
        }

        /// <summary>
        /// Act function to be called once each update.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <returns>actionEnded</returns>
        public override bool Act(BrainController brainController) {
            // Check if animation has ended.
            //Logger.LogMessage($"{brainController.gameObject.name}::FightAttackAction::Act;");
            return brainController.animationController.CheckCurrentAnimationEnded(animationTrigger);
        }
    }
}