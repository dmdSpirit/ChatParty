﻿using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Avatar brain for combat.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Brains/FighBrain")]
    public class FightBrain : BrainClass {
        [SerializeField]
        string ildeAnimationTrigger;
        [SerializeField]
        AvatarCombatAction runAction;
        [SerializeField]
        AvatarCombatAction hurtAction;
        [SerializeField]
        AvatarCombatAction deathAction;

        /// <summary>
        /// Initializing function for brain.
        /// </summary>
        /// <param name="brainController">BrainController</param>
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
            brainController.animationController.TriggerAnimation(ildeAnimationTrigger);
            // Set combat runSpeed to min run speed of Avatar.
            brainVariables.runSpeed = brainController.Stats.runSpeed.x;
            // HACK: We can't call GetNextAction yet, cause we dont have fightController passed to brain.
            // We will call first GetNextAction in SetFightController.
            // ---- brainVariables.nextAction = GetNextAction(brainController);
            brainVariables.turnEnded = false;
            brainController.avatarController.avatarStats.ShowHP = true;
            // FIXME: For first combat build. Add hp regeneration later.
            brainController.avatarController.avatarStats.ResetHP();
        }

        /// <summary>
        /// Update function to be called once each update.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <returns>actionEnded</returns>
        public override void UpdateBrain(BrainController brainController) {

            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            if (brainVariables.turn != false) {
                //Logger.LogMessage($"{brainController.gameObject.name}::UpdateBrain -- ")
                if (brainVariables.turnEnded)
                    EndTurn(brainController);
                else if (brainVariables.currentAction != null)
                    brainVariables.turnEnded = brainVariables.currentAction.Act(brainController);
            }
            else if (brainVariables.currentAction != null) {
                if (brainVariables.currentAction.Act(brainController)) {
                    brainVariables.currentAction = null;
                    //brainController.animationController.TriggerAnimation(ildeAnimationTrigger);
                }
            }
        }

        /// <summary>
        /// Saves current FightController to variables and rolls for initiative.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        /// <param name="fightController">FightController</param>
        /// <returns>Initiative</returns>
        public float SetFightController(BrainController brainController, FightController fightController, int teamID) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainVariables.fightController = fightController;
            brainVariables.nextAction = GetNextAction(brainController);
            brainVariables.TeamID = teamID;
            brainController.avatarController.avatarStats.OnDeath += fightController.OnDeath;
            return brainVariables.initiative;
        }

        /// <summary>
        /// Starts turn for this brain.
        /// </summary>
        /// <param name="brainController">BrainController</param>
        public void StartTurn(BrainController brainController) {
            // TODO: if the enemy is too close, step back a little to attack distance.
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainVariables.turn = true;
            brainVariables.turnEnded = false;
            // Check the enemy. It can be dead.
            if (brainVariables.currentEnemy.avatarController.avatarStats.IsAlive == false) {
                brainVariables.currentEnemy = brainVariables.fightController.GetClosestEnemy(brainController);
                Logger.LogMessage($"{brainController.gameObject.name}::StartTurn " +
                    $"-- enemy is dead. Getting new enemy : {brainVariables.currentEnemy}");
            }
            var distanceToEnemy = Mathf.Abs(brainController.transform.position.x - brainVariables.currentEnemy.transform.position.x);
            // FIXME: Change this check if any other moving action will be implemented.
            if (distanceToEnemy > brainController.Stats.attackDistance && brainVariables.nextAction != runAction)
                brainVariables.nextAction = GetNextAction(brainController);
            if (brainVariables.nextAction == null) {
                Logger.LogMessage($"{name}::StartTurn -- nextAction is null.", LogType.Error);
                return;
            }
            brainVariables.currentAction = brainVariables.nextAction;
            Logger.LogMessage($"{brainController.gameObject.name}::StartTurn -- currentEnemy = {brainVariables.currentEnemy.gameObject.name}; " +
                $"currentAction = {brainVariables.currentAction.name}");
            brainVariables.currentAction.InitAction(brainController);
            brainVariables.spriteSortOrder = brainController.animationController.SpriteSortOrder;
            // HACK: Hardcoded max sort order.
            // TODO: Also reorder text panels.
            brainController.animationController.SpriteSortOrder = 300;
        }

        private AvatarCombatAction GetNextAction(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            AvatarCombatAction nextAction;
            // FIXME: Check if Avatar need to attack other closest enemy.
            if (brainVariables.currentEnemy == null)
                brainVariables.currentEnemy = brainVariables.fightController.GetClosestEnemy(brainController);
            // FIXME: Rewrite this code using new clever action weight property.
            var distanceToEnemy = Mathf.Abs(brainController.transform.position.x - brainVariables.currentEnemy.transform.position.x);
            if (distanceToEnemy > brainController.Stats.attackDistance)
                nextAction = runAction;
            else
                nextAction = (AvatarCombatAction)GetRandomAction();
            return nextAction;
        }

        private void EndTurn(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            brainVariables.nextAction = GetNextAction(brainController);
            brainVariables.currentAction = null;
            brainVariables.turn = false;
            //brainController.animationController.TriggerAnimation(ildeAnimationTrigger);
            brainController.animationController.SpriteSortOrder = brainVariables.spriteSortOrder;
            brainVariables.fightController.EndTurn(lastFighter: brainController, cooldown: brainVariables.ActionCooldown);
        }

        public void TakeDamage(BrainController brainController, float damage, bool crit) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
            if (crit)
                Logger.LogMessage($"{brainController.gameObject.name} has taken {damage} damage! Its a crit!");
            else
                Logger.LogMessage($"{brainController.gameObject.name} has taken {damage} damage.");
            if (brainController.avatarController.avatarStats.TakeDamage(damage) == 0) {
                brainVariables.TeamID = 0;
                brainVariables.currentAction = deathAction;
            }
            else {
                brainVariables.currentAction = hurtAction;
            }
            brainVariables.currentAction.InitAction(brainController);

        }
        
        public void EndFight(BrainController brainController) {
            var brainVariables = brainController.GetComponent<FightBrainVariables>();
           // brainController.animationController.TriggerAnimation(ildeAnimationTrigger);
            brainController.avatarController.avatarStats.ShowHP = false;
            brainVariables.TeamID = 0;
        }
    }

    /// <summary>
    /// Variables for FightBrain.
    /// </summary>
    public class FightBrainVariables : MonoBehaviour {
        public FightController fightController;
        public BrainController currentEnemy;
        public AvatarCombatAction nextAction;
        public AvatarCombatAction currentAction;
        public bool turn = false;
        // Initiative is rolled once per battle.
        public float initiative;
        /// <summary>
        /// Show if it is this avatar's turn.
        /// </summary>
        public float ActionCooldown => nextAction == null ? 0 : nextAction.ActionCooldown;
        public float distanceToRun;
        public float runDirection;
        public float runSpeed;
        public bool turnEnded = false;
        public int spriteSortOrder;

        public int TeamID {
            get { return teamID; }
            set {
                if (value > 0) {
                    avatarController.teamIDText.text = value.ToString();
                    avatarController.teamIDPanel.enabled = true;
                    teamID = value;
                }
                else {
                    avatarController.teamIDText.text = "";
                    avatarController.teamIDPanel.enabled = false;
                }
            }
        }

        AvatarController avatarController;
        int teamID;

        private void Awake() {
            avatarController = GetComponent<AvatarController>();
        }
    }
}