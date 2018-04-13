using UnityEngine;
using System.Linq;
//using UnityEditor.Animations;

namespace dmdSpirit {
    /// <summary>
    /// Conponent that handles all Sprite animation.
    /// </summary>
    [RequireComponent(typeof(AvatarController))]
    public class AnimationController : MonoBehaviour {

        /// <summary>
        /// Direction of Sprite.
        /// </summary>
        public float Direction {
            get { return direction; }
            set {
                direction = Mathf.Sign(value);
                SpriteGO.transform.localScale = new Vector2(direction, 1);
            }
        }
        public int SpriteSortOrder {
            get { return spriteSortOrder; }
            set { SpriteGO.GetComponent<SpriteRenderer>().sortingOrder = value; spriteSortOrder = value; }
        }


        AvatarController avatarController;
        GameObject SpriteGO { get { return avatarController.spriteGO; } }
        Animator SpriteAnimator { get { return avatarController.spriteAnimator; } }

        float direction;
        int spriteSortOrder;


        private void Awake() {
            avatarController = GetComponent<AvatarController>();
        }

        /// <summary>
        /// Sends trigger to Sprite Animator.
        /// </summary>
        /// <param name="animationTrigger">Animation Trigger.</param>
        public void TriggerAnimation(string animationTrigger) {
            // All animations must have hasExitTime = false.
            //if (logType == DebugLogType.Full)
            //Logger.LogMessage($"{Time.time} :: {gameObject.name}::AnimationController -- Animation {animationTrigger} is triggered.");
            SpriteAnimator.SetTrigger(animationTrigger);
        }

        /// <summary>
        /// Check if current triggered animation has ended.
        /// </summary>
        /// <param name="animationTrigger">Animation Trigger</param>
        /// <returns>Wether animation stoped.</returns>
        public bool CheckCurrentAnimationEnded(string animationTrigger) {
            // FIXME: This method is not 100% stable.
            var currentClipName = SpriteAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            var normalizedTime = SpriteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //Logger.LogMessage($"{Time.time} :: {gameObject.name}::AnimationController -- currentClipName = {currentClipName}, normalizedTime = {normalizedTime}");
            return animationTrigger != currentClipName ? false : normalizedTime >= 1;
        }

        public AnimationEvent[] GetClipAnimationEvents(string animationTrigger) {
            var animationClips = SpriteAnimator.runtimeAnimatorController.animationClips;
            AnimationClip clip = animationClips.Where(t => t.name == animationTrigger).FirstOrDefault();
            if (clip == null) {
                Logger.LogMessage($"{gameObject.name}::AnimationController -- no clip with name {animationTrigger} found.", LogType.Error);
                return null;
            }
            return clip.events;
        }
    }
}
