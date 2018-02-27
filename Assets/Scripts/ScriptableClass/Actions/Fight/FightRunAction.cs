using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Run to enemy action from Avatar Combat Actions.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Fight/Run")]
    public class FightRunAction : AvatarCombatAction {
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
            //Logger.LogMessage($"{brainController.gameObject.name}::FightRunAction::InitAction -- distanceToRun = {brainVariables.distanceToRun};");
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
            //Logger.LogMessage($"{brainController.gameObject.name}::FightRunAction::Act -- runDistance = {runDistance};" +
            //    $" new distanceToRun = {brainVariables.distanceToRun}");
            return brainVariables.distanceToRun <= 0;
        }
    }

}
