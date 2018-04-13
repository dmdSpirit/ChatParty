using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Run to enemy action from Avatar Combat Actions.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Fight/Run")]
    public class FightRunAction : AvatarCombatAction {
        public string ildeAnimationTrigger;
        /// <summary>
        /// Initializing function for action.
        /// </summary>
        /// <param name="brainController">BrainController.</param>
        public override void InitAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainController.animationController.TriggerAnimation(animationTrigger);
            float distanceToEnemy = brainVariables.currentEnemy.transform.position.x - brainController.transform.position.x;
            brainVariables.runDirection = Mathf.Sign(distanceToEnemy);
            if (Mathf.Abs(distanceToEnemy) < brainController.Stats.attackDistance)
                brainVariables.distanceToRun = 0;
            else
                brainVariables.distanceToRun =
                    Mathf.Min(Mathf.Abs(distanceToEnemy) - brainController.Stats.attackDistance, brainController.Stats.maxCombatRunDistance);
            brainController.animationController.Direction = brainVariables.runDirection;
        }

        /// <summary>
        /// Act function to be called once each update.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <returns>actionEnded</returns>
        public override bool Act(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            float runDistance = Mathf.Min(brainVariables.runSpeed * Time.deltaTime, brainVariables.distanceToRun);
            brainVariables.distanceToRun -= runDistance;
            brainController.transform.Translate(runDistance * brainVariables.runDirection, 0, 0);
            bool actionEnded = brainVariables.distanceToRun <= 0;
            if(actionEnded)
                brainController.animationController.TriggerAnimation(ildeAnimationTrigger);
            // TODO: Decide what to do with small side steps.
            return actionEnded;
        }
    }

}
