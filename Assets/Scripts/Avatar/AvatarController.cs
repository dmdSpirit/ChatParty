using System;
using UnityEngine;
using UnityEngine.UI;

namespace dmdSpirit {
    /// <summary>
    /// Main controlling component for Avatar gameObject. Supports the workflow of other components.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(MessageQueue), typeof(AvatarStats))]
    [RequireComponent(typeof(BrainController))]
    public class AvatarController : MonoBehaviour {
        public Text teamIDText;
        public Image teamIDPanel;
        public string zombieSpriteName = "zombie";
        public bool InCombat => brainController.InCombat;

        [HideInInspector]
        public GameObject spriteGO;
        [HideInInspector]
        public Viewer viewer;
        [HideInInspector]
        public int spriteSortOrder;
        [HideInInspector]
        public Animator spriteAnimator;
        [HideInInspector]
        public AvatarStats avatarStats;
        [HideInInspector]
        public AnimationController animationController;
        [HideInInspector]
        public AnimationEventHandler animationEventHandler;
        [HideInInspector]
        public BrainController brainController;

        [SerializeField]
        Text nameText;
        
        MessageQueue messageQueue;
        Zombify zombify;
        string spriteName;

        public void TurnInto(TurnIntoClass turnIntoTarget = null) {
            if (turnIntoTarget != null) {
                CreateSprite(turnIntoTarget.spriteName);
                brainController.ChangeBrains(turnIntoTarget);
            }
            else {
                CreateSprite(spriteName);
                brainController.RestoreBrains();
            }
        }

        private void Awake() {
            animationController = GetComponent<AnimationController>();
            messageQueue = GetComponent<MessageQueue>();
            avatarStats = GetComponent<AvatarStats>();
            brainController = GetComponent<BrainController>();
            zombify = GetComponent<Zombify>();
        }

        private void Start() {
            gameObject.name = viewer.Name;
            if (nameText == null)
                Logger.LogMessage($"{gameObject.name}::AvatarController -- nameText field is not set.", LogType.Error);
            else
                nameText.text = viewer.Name;
            string spriteName = ViewerBaseController.Instance.GetSpriteName(viewer);
            CreateSprite(spriteName);
            // We have to call LoadStats manualy instead of implementing this in Start of AvatarStats because we can't
            // be sure its Start will be called after this one.
            avatarStats.LoadStats(avatarName: spriteName);
        }

        /// <summary>
        /// Destroys current Avatar.
        /// </summary>
        public void DestroyAvatar() {
            Destroy(gameObject);
        }

        /// <summary>
        /// Adds message to Avatar message queue.
        /// </summary>
        /// <param name="message">Message.</param>
        public void AddMessage(string message) {
            messageQueue.AddMessage(message);
        }

        /// <summary>
        /// Adds command to Avatar command queue.
        /// </summary>
        /// <param name="command">Command.</param>
        public void AddCommand(string command) {
            // TODO: Implement AddCommand method.
            // HACK: Now we have only one command "StartFight", no info about Avatar is needed.
            CommandController.Instance.AddCommand(command);
            return;
        }

        private void CreateSprite(string spriteName) {
            if (spriteGO == null)
                this.spriteName = spriteName;
            else
                Destroy(spriteGO);
            GameObject spritePrefab = Resources.Load<GameObject>($"SpritePrefabs/{spriteName}");
            if (spritePrefab == null) 
                Logger.LogMessage($"{gameObject.name} :: could not find spritePrefab called {spriteName}.", LogType.Error);
            else {
                spriteGO = Instantiate(spritePrefab, transform);
                animationController.SpriteSortOrder = spriteSortOrder;
                spriteAnimator = spriteGO.GetComponent<Animator>();
                if (spriteAnimator == null)
                    Logger.LogMessage($"{gameObject.name}::AvatarController::CreateSprite -- spriteAnimator is null", LogType.Error);
                animationEventHandler = spriteGO.GetComponent<AnimationEventHandler>();
                if (animationEventHandler == null)
                    Logger.LogMessage($"{gameObject.name}::AvatarController::CreateSprite -- animationEventHandler is null", LogType.Error);
            }
        }
    }
}