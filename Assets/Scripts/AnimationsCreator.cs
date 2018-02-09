using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class AnimationsCreator : MonoBehaviour {
	#if UNITY_EDITOR
	public GameObject baseSpritePrefab;

	public Object spriteSheet;
	public Sprite[] sprites;

	public AnimationClass[] animations;

	// FIXME: get avatarName from spriteSheet name.
	public string avatarName;

	Dictionary <AnimationClass, AnimationClip> animationsDictionary = 
		new Dictionary<AnimationClass, AnimationClip> ();



	public void LoadSprites(){
		if (spriteSheet != null) {
			string path = AssetDatabase.GetAssetPath (spriteSheet);
			sprites = AssetDatabase.LoadAllAssetsAtPath (path).OfType<Sprite> ().ToArray ();
			Debug.Log ("Sprites loaded successfully. Total number: " + sprites.Length);
			avatarName = spriteSheet.name;
		} else
			Debug.LogError ("The Sprite Sheet field is empty.");
	}

	public void CreateNewAnimations(){
		AnimationClip newAnimationClip;
		animationsDictionary.Clear ();
		if(AssetDatabase.IsValidFolder("Assets/AnimatorController/" + avatarName + "Animations") == false)
		AssetDatabase.CreateFolder ("Assets/AnimatorController", avatarName + "Animations");
		foreach(var animation in animations){
			newAnimationClip = CreateAnimation (animation);
			animationsDictionary.Add (animation, newAnimationClip);
		}

	}

	public void CreateAnimationController(){
		if(string.IsNullOrEmpty(avatarName)){
			Debug.LogError ("AnimationsCreator :: Avatar Name is empty.");
			return;
		}
		CreateNewAnimations ();
		UnityEditor.Animations.AnimatorState[] animatorStates = 
			new UnityEditor.Animations.AnimatorState[animations.Length];
		bool hasDefaultState = false;
		UnityEditor.Animations.AnimatorStateTransition transition;
		// Creates controller.
		var animationController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath (
			                          "Assets/AnimatorController/" + avatarName + ".controller");
		// Add root state machine.
		var rootStateMachine = animationController.layers[0].stateMachine;
		// Add parameters and states and transitions.
		for(int i=0; i<animations.Length; i++){
			animationController.AddParameter (animations[i].name, AnimatorControllerParameterType.Trigger);
			animatorStates [i] = rootStateMachine.AddState (animations [i].name);
			animatorStates [i].motion = animationsDictionary [animations [i]];
			transition = rootStateMachine.AddAnyStateTransition (animatorStates [i]);
			transition.duration = 0;
			transition.exitTime = 0;
			transition.AddCondition (UnityEditor.Animations.AnimatorConditionMode.If, 0, animations [i].name);
			if (animations [i].isDefault && hasDefaultState == false) {
				hasDefaultState = true;
				//rootStateMachine.AddEntryTransition (animatorStates [i]);
				rootStateMachine.defaultState = animatorStates [i];
			}
		}
		if(hasDefaultState == false)
			rootStateMachine.defaultState = animatorStates [0];
			//rootStateMachine.AddEntryTransition (animatorStates [0]);
		CreateSpritePrefab(animationController);	
	}

	public AnimationClip CreateAnimation (AnimationClass animationStats){
		int framesCount = animationStats.end - animationStats.start + 1;
		AnimationClip newAnimationClip = new AnimationClip ();
		AnimationClipSettings newSettings = new AnimationClipSettings ();
		newSettings.loopTime = animationStats.isLooping;
		newAnimationClip.frameRate = framesCount*2;
		AnimationUtility.SetAnimationClipSettings(newAnimationClip, newSettings);
		EditorCurveBinding curveBinding = new EditorCurveBinding ();
		curveBinding.type = typeof(SpriteRenderer);
		curveBinding.path = "";
		curveBinding.propertyName = "m_Sprite";
		ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[framesCount];
		for(int i=0; i<framesCount; i++){
			keyFrames [i] = new ObjectReferenceKeyframe ();
			keyFrames [i].time = (float)i/newAnimationClip.frameRate;
			keyFrames [i].value = sprites [animationStats.start+i];
		}
		AnimationUtility.SetObjectReferenceCurve (newAnimationClip, curveBinding, keyFrames);
		AssetDatabase.CreateAsset(newAnimationClip, "Assets/AnimatorController/" + avatarName+ "Animations/"
			+animationStats.name+".anim");
		AssetDatabase.SaveAssets();

		return newAnimationClip;
	}


	public void CreateSpritePrefab(UnityEditor.Animations.AnimatorController animationController ){
		GameObject newPrefab = Instantiate (baseSpritePrefab);
		newPrefab.name = avatarName;
		Animator animator = newPrefab.GetComponent<Animator> ();
		animator.runtimeAnimatorController = animationController;
		PrefabUtility.CreatePrefab ("Assets/Resources/SritePrefabs/" + avatarName + ".prefab", 
			newPrefab, ReplacePrefabOptions.ReplaceNameBased);
		DestroyImmediate (newPrefab);
	}
	#endif
}