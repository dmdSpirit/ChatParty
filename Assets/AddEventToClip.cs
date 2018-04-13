using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace dmdSpirit {
    public class AddEventToClip : MonoBehaviour {
        [SerializeField]
        AnimationClip[] animationClipsArray;
        [SerializeField]
        string functionName = "OnAnimationEnded";

        private int? HasEvent(AnimationEvent[] eventArray) {
            int? result = null;
            for(int i=0; i<eventArray.Length; i++) {
                if (eventArray[i].functionName == functionName) {
                    result = i;
                    break;
                }
            }
            return result;
        }
#if UNITY_EDITOR
        /// <summary>
        /// Add event to the end of all clips from animationClipsArray.
        /// </summary>
        public void AddEventToEnd() {
            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.functionName = functionName;
            foreach (var animationClip in animationClipsArray) {
                var clipName = animationClip.name;
                var eventArray = AnimationUtility.GetAnimationEvents(animationClip);
                var eventIndex = HasEvent(eventArray);
                if (eventIndex == null) {
                    animationEvent.stringParameter = clipName;
                    animationEvent.time = animationClip.length;
                    Array.Resize(ref eventArray, eventArray.Length + 1);
                    eventArray[eventArray.Length - 1] = animationEvent;
                    AnimationUtility.SetAnimationEvents(animationClip, eventArray);
                    Logger.LogMessage($"{functionName} event has been added to {animationClip.name} clip.");
                }
                else {
                    if (eventArray[(int)eventIndex].stringParameter != clipName) {
                        eventArray[(int)eventIndex].stringParameter = clipName;
                        AnimationUtility.SetAnimationEvents(animationClip, eventArray);
                        Logger.LogMessage($"Clip {animationClip.name} already has {functionName} event but stringParameter was changed " +
                            $"to {eventArray[(int)eventIndex].stringParameter}.");
                    }
                    else
                        Logger.LogMessage($"Clip {animationClip.name} already has {functionName} event.");
                }
            }
            animationClipsArray = null;
        }
#endif
    }

}
