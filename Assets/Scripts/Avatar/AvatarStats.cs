using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Component responsible for handling Avatar stats.
    /// </summary>
    public class AvatarStats : MonoBehaviour {
        public AvatarStatsClass stats;

        /// <summary>
        /// Loads Avatar Stats from scriptable object.
        /// </summary>
        /// <param name="avatarName">Avatar Name.</param>
        public void LoadStats(string avatarName) {
            stats = Resources.Load<AvatarStatsClass>($"AvatarStats/{avatarName}");
        }

    }
}