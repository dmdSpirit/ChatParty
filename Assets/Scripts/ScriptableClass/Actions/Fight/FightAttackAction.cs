using UnityEngine;
using System.Collections.Generic;

namespace dmdSpirit {
    /// <summary>
    /// Simple attack from Avatar Combat Actions.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Fight/Attack")]
    public class FightAttackAction : AvatarCombatAction {
        public float baseDamageMultiplier;
        // TODO: Check if enemy is close enough or set weight to 0.

        /// <summary>
        /// Initializing function for action.
        /// </summary>
        /// <param name="brainController">BrainController.</param>
        public override void InitAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            if (CheckClipEvents(brainController) == false)
                return;
            brainController.animationController.TriggerAnimation(animationTrigger);
            float attackDirection = brainController.transform.position.x > brainVariables.currentEnemy.transform.position.x ? -1 : 1;
            brainController.animationController.Direction = attackDirection;
            brainController.animationEventHandler.Init();
            CheckClipEvents(brainController);
        }


        /// <summary>
        /// Act function to be called once each update.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <returns>actionEnded</returns>
        public override bool Act(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            if (brainController.animationEventHandler.damageDealt
                && brainController.animationEventHandler.animationTrigger == animationTrigger) {
                DealDamage(brainController);
                brainController.animationEventHandler.damageDealt = false;
            }
            var animationEnded = brainController.animationEventHandler.animationEnded;
            var sameAnimationTrigger = brainController.animationEventHandler.animationTrigger == animationTrigger;
            Logger.LogMessage($"{brainController.gameObject.name}::FightAttackAction::Act -- animationEnded = {animationEnded}, " +
                $"sameAnimationTrigger = {sameAnimationTrigger}");
            bool enemyIsActing = false;
            var enemyFightVariables = brainVariables.currentEnemy.GetComponent<FightBrainVariables>();
            if (enemyFightVariables == null)
                Logger.LogMessage($"{brainVariables.currentEnemy.gameObject.name} is in fight but has no FightBrainVariables", LogType.Error);
            else 
                enemyIsActing = enemyFightVariables.currentAction != null;
            return animationEnded && sameAnimationTrigger && enemyIsActing == false;
        }

        private void DealDamage(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            FightBrain fightBrain = (FightBrain)brainController.currentBrain;
            var avatarStats = brainController.Stats;
            int crit = Random.value <= avatarStats.critChance ? 1 : 0;
            float damage = Random.Range(avatarStats.damage.x, avatarStats.damage.y) * (1 + crit * (avatarStats.critMultiplayer - 1));
            fightBrain.TakeDamage(brainVariables.currentEnemy, damage, crit==1);
        }
    }
}