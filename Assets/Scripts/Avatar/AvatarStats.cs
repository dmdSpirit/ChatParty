using System;
using UnityEngine;
using UnityEngine.UI;

namespace dmdSpirit {
    /// <summary>
    /// Component responsible for handling Avatar stats.
    /// </summary>
    public class AvatarStats : MonoBehaviour {
        public AvatarStatsClass stats;
        // TODO: Show currentHP
        public bool IsAlive => currentHP != 0;
        public bool ShowHP {
            get { return showHP; }
            set {
                showHP = value;
                hpPanel.enabled = showHP;
                hpText.text = showHP ? $"{Math.Ceiling(currentHP)}/{stats.maxHP}" : "";
            }
        }

        [SerializeField]
        float currentHP;
        [SerializeField]
        Text hpText;
        [SerializeField]
        Image hpPanel;

        public event Action<BrainController> OnDeath;

        BrainController brainController;
        bool showHP;

        private void Awake() {
            brainController = GetComponent<BrainController>();
            if (hpText == null)
                Logger.LogMessage($"{gameObject.name}::AvatarStats -- hpText feild is not set.", LogType.Error);
            if (hpPanel == null)
                Logger.LogMessage($"{gameObject.name}::AvatarStats -- hpPanel feild is not set.", LogType.Error);
        }

        /// <summary>
        /// Loads Avatar Stats from scriptable object.
        /// </summary>
        /// <param name="avatarName">Avatar Name.</param>
        public void LoadStats(string avatarName) {
            stats = Resources.Load<AvatarStatsClass>($"AvatarStats/{avatarName}");
            currentHP = stats.maxHP;
            ShowHP = false;
        }

        public float TakeDamage(float damage) {
            if (currentHP == 0)
                return 0;
            currentHP = Mathf.Max(0, currentHP - damage);
            if(currentHP == 0) 
                Death();
            ShowHP = true;
            return currentHP;
        }

        private void Death() {
            Logger.LogMessage($"{gameObject.name} is dead.");
            if (OnDeath != null)
                OnDeath(brainController);
            ShowHP = false;
        }

        public void ResetHP() {
            currentHP = stats.maxHP;
        }
    }
}