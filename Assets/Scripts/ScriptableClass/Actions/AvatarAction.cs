using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Base class for all Avatar actions.
    /// </summary>
    public abstract class AvatarAction : ScriptableObject {
        public string animationTrigger;
        // I add property to be able to alter action weight depending on different parameters.
        public virtual int ActionWeight => baseActionWeight;

        [SerializeField]
        protected int baseActionWeight;

        /// <summary>
        /// Initializing function for action.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        public abstract void InitAction(BrainController brainController);

        /// <summary>
        /// Act function to be called once each update.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <returns>actionEnded</returns>
        public abstract bool Act(BrainController brainController);
    }
}