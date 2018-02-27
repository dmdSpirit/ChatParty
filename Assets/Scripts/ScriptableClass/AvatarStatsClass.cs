using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Scriptable object for storing Avatar stats.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/ViewerStats")]
    public class AvatarStatsClass : ScriptableObject {
        // FIXME: Create getters for stats for future effects like speed_slowed.

        // Movement.
        public Vector2 walkSpeed;
        public Vector2 runSpeed;
        public Vector2 walkTravelDistance;
        public Vector2 runTravelDistance;
        public Vector2 idleTime;

        // Combat.
        public int maxHP;
        public float attackDistance;
        public float attackCooldown;
        public float maxCombatRunDistance;
        public Vector2 damage;
        public float armor;
        public float critChance;
        public float critMultiplayer;
    }
}
