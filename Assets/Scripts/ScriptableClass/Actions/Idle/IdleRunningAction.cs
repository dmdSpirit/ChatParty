using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Running for some amount of time action from Avatar Actions.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Idle/Run")]
    public class IdleRunningAction : AvatarAction {
        /// <summary>
        /// Initializing function for action.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        public override void InitAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<IdleBrainVariables>();
            var avatarStats = brainController.Stats;
            Vector2 position = brainController.transform.position;
            float leftBorder = BackgroundController.Instance.leftBorder.position.x;
            float rightBorder = BackgroundController.Instance.rightBorder.position.x;
            float newDistance = Random.Range(avatarStats.runTravelDistance.x, avatarStats.runTravelDistance.y)
                * (Random.value > 0.5 ? 1 : -1);
            float newDestinationPoint = position.x + newDistance;
            newDestinationPoint = Mathf.Clamp(newDestinationPoint, leftBorder, rightBorder);
            newDistance = newDestinationPoint - position.x;
            brainController.animationController.Direction = newDistance >= 0 ? 1 : -1;
            brainVariables.currentDistance = Mathf.Abs(newDistance);
            brainVariables.currentSpeed = Random.Range(avatarStats.runSpeed.x, avatarStats.runSpeed.y);
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
                Vector2 newTransform = brainController.transform.position;
                float movement = Mathf.Min(brainVariables.currentDistance, brainVariables.currentSpeed * Time.deltaTime);
                newTransform.x += brainController.animationController.Direction * movement;
                brainController.transform.position = newTransform;
                brainVariables.currentDistance -= movement;
                return false;
            }
            else
                return true;
        }
    }
}
