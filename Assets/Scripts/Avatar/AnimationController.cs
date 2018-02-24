using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    AvatarController avatarController;
    GameObject SpriteGO { get { return avatarController.spriteGO; } }
    Animator SpriteAnimator { get { return avatarController.spriteAnimator; } }
    float direction;


    private void Awake() {
        avatarController = GetComponent<AvatarController>();
    }

    /// <summary>
    /// Sends trigger to Sprite Animator.
    /// </summary>
    /// <param name="animationTrigger">Animation Trigger.</param>
    public void TriggerAnimation(string animationTrigger){
        // All animations must have hasExitTime = false.
        SpriteAnimator.SetTrigger(animationTrigger);
    }

    public bool CheckCurrentAnimationEnded() {
        return SpriteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }
}
