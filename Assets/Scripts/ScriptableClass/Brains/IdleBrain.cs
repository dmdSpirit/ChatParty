using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Avatar brain for idle behaviour.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Brains/IdleBrain")]
    public class IdleBrain : BrainClass {
        /// <summary>
        /// Initializing function for brain.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        public override void InitBrain(BrainController brainController) {
            var brainVariables = brainController.GetComponent<IdleBrainVariables>();
            if (brainVariables == null)
                brainVariables = brainController.gameObject.AddComponent<IdleBrainVariables>();
            if (actionList.Length == 0)
                Logger.LogMessage($"{name}::InitBrain -- actionList is empty.", LogType.Error);
            else {
                brainVariables.currentAction = GetRandomAction();
                brainVariables.currentAction.InitAction(brainController);
            }

        }

        /// <summary>
        /// Update function to be called once each update.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <returns>actionEnded</returns>
        public override void UpdateBrain(BrainController brainController) {
            var brainVariables = brainController.GetComponent<IdleBrainVariables>();
            if (brainVariables == null) {
                Logger.LogMessage($"{name}::Update -- brainVariables are not found. Brain has not been initialized", LogType.Error);
                return;
            }
            if (brainVariables.currentAction.Act(brainController)) {
                brainVariables.currentAction = GetRandomAction();
                brainVariables.currentAction.InitAction(brainController);
            }
        }
    }

    /// <summary>
    /// Variables for IdleBrain.
    /// </summary>
    public class IdleBrainVariables : MonoBehaviour {
        public AvatarAction currentAction;
        public float currentSpeed;
        public float currentDistance;
    }
}