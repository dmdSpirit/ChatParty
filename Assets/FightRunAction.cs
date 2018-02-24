using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dmdspirit.newScript {
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Fight/Run")]
    public class FightRunAction : AvatarCombatAction {
        public override void InitAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainController.animationController.TriggerAnimation(animationTrigger);
            float distanceToEnemy = brainVariables.currentEnemy.transform.position.x - brainController.transform.position.x;
            brainVariables.runDirection = Mathf.Sign(distanceToEnemy);
            brainVariables.distanceToRun = 
                Mathf.Min(Mathf.Abs(distanceToEnemy) - brainController.Stats.attackDistance, brainController.Stats.maxCombatRunDistance);
        }

        public override bool Act(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            float runDistance = Mathf.Min(brainVariables.runSpeed * Time.deltaTime, brainVariables.distanceToRun);
            brainVariables.distanceToRun -= runDistance;
            brainController.transform.Translate(runDistance * brainVariables.runDirection, 0, 0);
            return brainVariables.distanceToRun == 0;
        }
    }

}
