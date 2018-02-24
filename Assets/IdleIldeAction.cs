using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dmdspirit.newScript {
    [CreateAssetMenu(menuName = "ScriptableObject/Actions/Idle/Idle")]
    public class IdleIldeAction : AvatarAction {
        public override void InitAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<IdleBrainVariables>();
            brainController.animationController.Direction = Random.Range(-1, 1) > 0 ? 1 : -1;
            Vector2 idleTime = brainController.Stats.idleTime;
            brainVariables.currentDistance = Random.Range(idleTime.x, idleTime.y);
            brainController.animationController.TriggerAnimation(animationTrigger);
        }

        public override bool Act(BrainController brainController) {
            var brainVariables = brainController.GetComponent<IdleBrainVariables>();
            if (brainVariables.currentDistance > 0) {
                brainVariables.currentDistance -= Time.deltaTime;
                return false;
            }
            else
                return true;
        }
    }
}
