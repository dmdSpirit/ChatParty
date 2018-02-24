using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dmdspirit.newScript {

    public abstract class AvatarAction : ScriptableObject {
        public string animationTrigger;
        public int actionWeight;

        public abstract void InitAction(BrainController brainController);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brainController"></param>
        /// <returns>actionEnded</returns>
        public abstract bool Act(BrainController brainController);
    }
}
