using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Staying idle for some amount of time action from Avatar Actions.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Idle/Idle")]
    public class IdleIldeAction : AvatarAction {
        /// <summary>
        /// Initializing function for action.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        public override void InitAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<IdleBrainVariables>();
            //brainController.animationController.Direction = Random.Range(-1, 1) > 0 ? 1 : -1;
            Vector2 idleTime = brainController.Stats.idleTime;
            brainVariables.currentDistance = Random.Range(idleTime.x, idleTime.y);
            brainController.animationController.TriggerAnimation(animationTrigger);
        }

        /// <summary>
        /// Act function to be called once each update.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <returns>actionEnded</returns>
        public override bool Act(BrainController brainController) {
            var brainVariables = brainController.GetComponent<IdleBrainVariables>();
            if (brainVariables.currentDistance > 0) {
                brainVariables.currentDistance -= Time.deltaTime;
                return false;
            }
            else
                return true;
        }
    }
}
