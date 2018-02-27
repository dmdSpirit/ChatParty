using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Base class for all Avatar brains.
    /// </summary>
    public abstract class BrainClass : ScriptableObject {
        [SerializeField]
        protected AvatarAction[] actionList;

        /// <summary>
        /// Initializing function for brain.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        public abstract void InitBrain(BrainController brainController);

        /// <summary>
        /// Update function to be called once each update.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <returns>actionEnded</returns>
        public abstract void UpdateBrain(BrainController brainController);

        protected AvatarAction GetRandomAction() {
            if (actionList.Length != 0) {
                int totalWeight = CalculateActionsWeight();
                float t = Random.Range(0, totalWeight);
                int newBehaviour = 0;
                int currentWeight = 0;
                for (int i = 0; i < actionList.Length; i++) {
                    if (t < currentWeight + actionList[i].ActionWeight) {
                        newBehaviour = i;
                        break;
                    }
                    currentWeight += actionList[i].ActionWeight;
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
                totalWeight += action.ActionWeight;
            return totalWeight;
        }
    }
}