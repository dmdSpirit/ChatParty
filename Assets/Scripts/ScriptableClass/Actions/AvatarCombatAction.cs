using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Base class for all Avatar combat actions.
    /// </summary>
    public abstract class AvatarCombatAction : AvatarAction {
        public virtual float ActionCooldown => baseActionCooldown;

        [SerializeField]
        protected float baseActionCooldown;
    }
}