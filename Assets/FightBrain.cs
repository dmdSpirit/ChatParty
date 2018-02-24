using System.Collections.Generic;
using UnityEngine;

namespace dmdspirit.newScript {

    [CreateAssetMenu(menuName = "ScriptableObject/Brains/FighBrain")]
    public class FightBrain : BrainClass {
        public string ildeAnimationTrigger;
        public AvatarCombatAction runAction;

       /* private void OnValidate() {
            List<AvatarCombatAction> avatarCombatList = new List<AvatarCombatAction>();
            foreach (var avatarAction in actionList)
                if (avatarAction.GetType() == typeof(AvatarCombatAction))
                    avatarCombatList.Add((AvatarCombatAction)avatarAction);
                else
                    Logger.LogMessage($"You are trying to add noncombat action ({avatarAction.name}) to {name}.");
            actionList = avatarCombatList.ToArray();
        }*/

        public override void InitBrain(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            if (brainVariables == null)
                brainVariables = brainController.gameObject.AddComponent<FightBrainVariables>();
            if (actionList.Length == 0)
                Logger.LogMessage($"{name}::InitBrain -- actionList is empty.", LogType.Error);
            if (runAction == null)
                Logger.LogMessage($"{name}::InitBrain -- runAction is not set.", LogType.Error);
            if (string.IsNullOrEmpty(ildeAnimationTrigger))
                Logger.LogMessage($"{name}::InitBrain -- ildeAnimationTrigger is not set.", LogType.Error);
            // FIXME: Make initiative roll dependent on some stat like dexterity.
            brainVariables.initiative = UnityEngine.Random.value;
            brainVariables.nextAction = (AvatarCombatAction)GetRandomAction();
            brainController.animationController.TriggerAnimation(ildeAnimationTrigger);
            // Set combat runSpeed to max run speed of Avatar.
            brainVariables.runSpeed = brainController.Stats.runSpeed.y;
            brainVariables.nextAction = GetNextAction(brainController);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <param name="fightController">FightController</param>
        /// <returns>Initiative</returns>
        public float SetFightController(BrainController brainController, FightController fightController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainVariables.fightController = fightController;
            return brainVariables.initiative;
        }

        public override void UpdateBrain(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            if (brainVariables.turn == false)
                return;
            if (brainVariables.currentAction != null)
                if (brainVariables.currentAction.Act(brainController))
                    EndTurn(brainController);
        }

        public void StartTurn(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainVariables.turn = true;
            var distanceToEnemy = Mathf.Abs(brainController.transform.position.x - brainVariables.currentEnemy.transform.position.x);
            // Change this check if any other moving action will be implemented.
            if (distanceToEnemy > brainController.Stats.attackDistance && brainVariables.nextAction != runAction)
                brainVariables.nextAction = GetNextAction(brainController);
            if (brainVariables.nextAction == null) {
                Logger.LogMessage($"{name}::StartTurn -- nextAction is null.", LogType.Error);
                return;
            }
            brainVariables.currentAction = brainVariables.nextAction;
            brainVariables.currentAction.InitAction(brainController);
        }

        private void EndTurn(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainVariables.nextAction = GetNextAction(brainController);
            brainVariables.turn = false;
            brainController.animationController.TriggerAnimation(ildeAnimationTrigger);
            brainVariables.fightController.EndTurn(lastFighter: brainController, cooldown: brainVariables.ActionCooldown);
        }

        private AvatarCombatAction GetNextAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            AvatarCombatAction nextAction;
            if (brainVariables.currentEnemy == null) 
                brainVariables.currentEnemy = brainVariables.fightController.GetClosestEnemy(brainController);
            var distanceToEnemy = Mathf.Abs(brainController.transform.position.x - brainVariables.currentEnemy.transform.position.x);
            if (distanceToEnemy > brainController.Stats.attackDistance)
                nextAction = runAction;
            else
                nextAction = (AvatarCombatAction)GetRandomAction();
            return nextAction;
        }
    }

    public class FightBrainVariables : MonoBehaviour {
        public FightController fightController;
        public BrainController currentEnemy;
        public AvatarCombatAction nextAction;
        public AvatarCombatAction currentAction;
        // Initiative is rolled once per battle.
        public float initiative;
        /// <summary>
        /// Show if it is this avatar's turn.
        /// </summary>
        public bool turn = false;
        public float ActionCooldown { get { return nextAction == null ? 0 : nextAction.actionCooldown; } }
        public float runSpeed;
        public float distanceToRun;
        public float runDirection;
    }
}
