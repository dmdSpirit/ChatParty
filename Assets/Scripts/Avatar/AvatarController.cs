using UnityEngine;
using UnityEngine.UI;

namespace dmdSpirit {
    /// <summary>
    /// Main controlling component for Avatar gameObject. Supports the workflow of other components.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(MessageQueue), typeof(AvatarStats))]
    public class AvatarController : MonoBehaviour {
        public Text teamIDText;
        public Image teamIDPanel;

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

        [SerializeField]
        Text nameText;
        
        MessageQueue messageQueue;

        private void Awake() {
            animationController = GetComponent<AnimationController>();
            messageQueue = GetComponent<MessageQueue>();
            avatarStats = GetComponent<AvatarStats>();
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
            return;
        }

        private void CreateSprite(string spriteName) {
            GameObject spritePrefab = Resources.Load<GameObject>($"SpritePrefabs/{spriteName}");
            if (spritePrefab == null) 
                Logger.LogMessage($"{gameObject.name} :: could not find spritePrefab called {spriteName}.", LogType.Error);
            else {
                spriteGO = Instantiate(spritePrefab, transform);
                animationController.SpriteSortOrder = spriteSortOrder;
                spriteAnimator = spriteGO.GetComponent<Animator>();
            }
        }
    }
}