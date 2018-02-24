using UnityEngine;

namespace dmdspirit.newScript {

    public abstract class BrainClass : ScriptableObject {
        [SerializeField]
        protected AvatarAction[] actionList;

        public abstract void InitBrain(BrainController brainController);
        public abstract void UpdateBrain(BrainController brainController);

        protected AvatarAction GetRandomAction() {
            if (actionList.Length != 0) {
                int totalWeight = CalculateActionsWeight();
                float t = Random.Range(0, totalWeight);
                int newBehaviour = 0;
                int currentWeight = 0;
                for (int i = 0; i < actionList.Length; i++) {
                    if (t < currentWeight + actionList[i].actionWeight) {
                        newBehaviour = i;
                        break;
                    }
                    currentWeight += actionList[i].actionWeight;
                }
                return actionList[newBehaviour];
            }
            else {
                Logger.LogMessage($"{name}::GetRandomAction -- brainVariables are not found. Brain has not been initialized", LogType.Error);
                return null;
            }
        }

        protected int CalculateActionsWeight() {
            int totalWeight = 0;
            foreach (var action in actionList)
                totalWeight += action.actionWeight;
            return totalWeight;
        }
    }

}
