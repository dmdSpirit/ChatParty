using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace dmdSpirit {
    public class AnimationEventHandler : MonoBehaviour {
        // TODO: Rewrite using behaviour components on mechanim states.
        public bool animationEnded;
        public bool damageDealt;
        public string animationTrigger;

        public void Init() {
            animationEnded = false;
            damageDealt = false;
            animationTrigger = "";
        }

        public void OnDamageDealt(string animationTrigger) {
            if (string.IsNullOrEmpty(animationTrigger))
                Logger.LogMessage($"{gameObject.name}::AnimationEventHandler::OnDamageDealt -- animationTrigger is not set");
            damageDealt = true;
            this.animationTrigger = animationTrigger;
            //Logger.LogMessage($"{gameObject.name}::AnimationEventHandler::OnDamageDealt triggered");

        }

        public void OnAnimationEnded(string animationTrigger) {
            if (string.IsNullOrEmpty(animationTrigger))
                Logger.LogMessage($"{gameObject.name}::AnimationEventHandler::OnAnimationEnded -- animationTrigger is not set");
            animationEnded = true;
            this.animationTrigger = animationTrigger;
            //Logger.LogMessage($"{gameObject.name}::AnimationEventHandler::OnAnimationEnded triggered");
        }
    }
}
