using UnityEngine;

namespace dmdspirit.newScript {
    [CreateAssetMenu(menuName = "ScriptableObject/Brains/IdleBrain")]
    public class IdleBrain : BrainClass {

        public override void InitBrain(BrainController brainController) {
            var brainVariables = brainController.GetComponent<IdleBrainVariables>();
            if (brainVariables == null)
                brainVariables = brainController.gameObject.AddComponent<IdleBrainVariables>();
            if (actionList.Length == 0)
                Logger.LogMessage($"{name}::InitBrain -- actionList is empty.", LogType.Error);
            else
                brainVariables.currentAction = GetRandomAction();

        }

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

    public class IdleBrainVariables : MonoBehaviour {
        public AvatarAction currentAction;
        public float currentSpeed;
        public float currentDistance;
    }
}
