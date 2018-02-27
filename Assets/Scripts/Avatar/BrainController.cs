using UnityEngine;

namespace dmdSpirit {
    [RequireComponent(typeof(AvatarController))]
    public class BrainController : MonoBehaviour {
        public AvatarStatsClass Stats => avatarController.avatarStats.stats;
        public AnimationController animationController => avatarController.animationController;

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