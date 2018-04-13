using UnityEngine;
using System.Collections.Generic;

namespace dmdSpirit {
    /// <summary>
    /// Base class for all Avatar combat actions.
    /// </summary>
    public abstract class AvatarCombatAction : AvatarAction {
        public string[] eventFunctionNames = { "OnAnimationEnded" };

        public virtual float ActionCooldown => baseActionCooldown;

        [SerializeField]
        protected float baseActionCooldown;

        protected virtual bool CheckClipEvents(BrainController brainController) {
            var eventsList = brainController.animationController.GetClipAnimationEvents(animationTrigger);
            if (eventsList == null)
                return false;
            Dictionary<string, bool> eventDictionary = new Dictionary<string, bool>();
            foreach (var eventFunctionName in eventFunctionNames) 
                eventDictionary.Add(eventFunctionName, false);
            foreach (var clipEvent in eventsList) 
                if (eventDictionary.ContainsKey(clipEvent.functionName))
                    eventDictionary[clipEvent.functionName] = true;
            bool result = true;
            foreach (var hasEvent in eventDictionary) 
                if (hasEvent.Value == false) {
                    result = false;
                    Logger.LogMessage($"{brainController.gameObject.name}::FightAttackAction " +
                        $"-- {hasEvent.Key} animation event is missing.", LogType.Error);
                }
            return result;
        }
    }
}