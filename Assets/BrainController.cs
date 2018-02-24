using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dmdspirit.newScript {
    [RequireComponent(typeof(AvatarController))]
    public class BrainController : MonoBehaviour {
        // FIXME: Create getters for stats for future effects like speed_slowed.
        public AvatarStatsClass Stats { get { return avatarController.avatarStats.stats; } }
        public AnimationController animationController { get { return avatarController.animationController; } }

        [SerializeField]
        IdleBrain idleBrain;
        [SerializeField]
        FightBrain fightBrain;

        BrainClass currentBrain;
        AvatarController avatarController;

        private void Awake() {
            avatarController = GetComponent<AvatarController>();
        }

        private void Start() {
            ChangeBrain(idleBrain);
        }

        private void Update() {
            if (currentBrain != null)
                currentBrain.UpdateBrain(this);
            else
                Logger.LogMessage($"{gameObject.name}::BrainController::Update -- currentBrain is null", LogType.Error);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fightController">FightController</param>
        /// <returns>Initiative</returns>
        public float StartFight(FightController fightController) {
            if (ChangeBrain(fightBrain))
                return fightBrain.SetFightController(this, fightController);
            return 0;
        }

        public void StartTurn() {
            if (currentBrain == fightBrain)
                fightBrain.StartTurn(this);
            else
                Logger.LogMessage($"{gameObject.name}::BrainController -- StartTurn is called when Avatar is not fighting.");
        }
    }

}
