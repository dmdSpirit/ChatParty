using System;
using UnityEngine;

namespace dmdSpirit {
    [RequireComponent(typeof(AvatarController))]
    public class BrainController : MonoBehaviour {
        // HACK: Needed to avatarStats. Remove after fix.
        public AvatarController avatarController;
        // FIXME: I need reference to avatarController.avatarStats.
        public AvatarStatsClass Stats => avatarController.avatarStats.stats;
        public AnimationController animationController => avatarController.animationController;
        public AnimationEventHandler animationEventHandler => avatarController.animationEventHandler;
        public bool InCombat => currentBrain == fightBrain;

        //[HideInInspector]
        public BrainClass currentBrain;

        [SerializeField]
        IdleBrain idleBrain;
        [SerializeField]
        FightBrain fightBrain;

        IdleBrain oldIdleBrain;
        FightBrain oldFightBrain;

        private void Awake() {
            avatarController = GetComponent<AvatarController>();
        }

        private void Start() {
            ChangeBrain(idleBrain);
            oldIdleBrain = idleBrain;
            oldFightBrain = fightBrain;
        }

        private void Update() {
            if (currentBrain != null)
                currentBrain.UpdateBrain(this);
            else
                Logger.LogMessage($"{gameObject.name}::BrainController::Update -- currentBrain is null", LogType.Error);
        }

        internal void ChangeBrains(TurnIntoClass turnIntoTarget) {
            // TODO: Check current brain and switch it to new. (Current brain hotSwitch)
            if (turnIntoTarget.idleBrain != null)
                idleBrain = turnIntoTarget.idleBrain;
            if (turnIntoTarget.fightBrain != null)
                fightBrain = turnIntoTarget.fightBrain;
        }

        internal void RestoreBrains() {
            // TODO: Check current brain and switch it to new. (Current brain hotSwitch)
            idleBrain = oldIdleBrain;
            fightBrain = oldFightBrain;
        }

        /// <summary>
        /// Changes AvatarBrain to fightBrain and prepares Avatar for combat.
        /// </summary>
        /// <param name="fightController">FightController</param>
        /// <returns>Initiative</returns>
        public float StartFight(FightController fightController, int teamID) {
            if (ChangeBrain(fightBrain))
                return fightBrain.SetFightController(this, fightController, teamID);
            return 0;
        }

        public void EndFight() {
            fightBrain.EndFight(this);
            ChangeBrain(idleBrain);
        }

        /// <summary>
        /// Starts this Avatar's combat turn.
        /// </summary>
        public void StartTurn() {
            if (currentBrain == fightBrain)
                fightBrain.StartTurn(this);
            else
                Logger.LogMessage($"{gameObject.name}::BrainController -- StartTurn is called when Avatar is not fighting.");
        }

        private bool ChangeBrain(BrainClass newBrain) {
            if (newBrain != null) {
                currentBrain = newBrain;
                currentBrain.InitBrain(this);
                return true;
            }
            else {
                Logger.LogMessage($"{gameObject.name}::BrainController -- trying to change brain to null-object.", LogType.Error);
                return false;
            }
        }
    }
}