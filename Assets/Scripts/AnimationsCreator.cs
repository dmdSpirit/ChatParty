using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.Animations;

/// <summary>
/// Component for creating ready-to-use Sprite prefabs with animations from spritesheet.
/// </summary>
public class AnimationsCreator : MonoBehaviour {
	#if UNITY_EDITOR
	public GameObject baseSpritePrefab;
	public Object spriteSheet;
	public AnimationClass[] animations;

	string avatarName;
	Sprite[] sprites;
	Dictionary <AnimationClass, AnimationClip> animationsDictionary = 
		new Dictionary<AnimationClass, AnimationClip> ();

	/// <summary>
	/// Loads sprites from spriteSheet to array.
	/// </summary>
	public void LoadSprites(){
		if (spriteSheet != null) {
			avatarName = spriteSheet.name;
			string path = AssetDatabase.GetAssetPath (spriteSheet);
			sprites = AssetDatabase.LoadAllAssetsAtPath (path).OfType<Sprite> ().ToArray ();
			Debug.Log ("Sprites loaded successfully. Total number: " + sprites.Length);
			if(sprites.Length<2)
				Debug.LogWarning("SpriteSheet contains only one sprite. Check if it was properly sliced.");
		} else
			Debug.LogError ("The Sprite Sheet field is empty.");
	}

	/// <summary>
	/// Creates ready-to-use sprite prefab and animation controller.
	/// </summary>
	public void CreateAnimationController(){
		if (string.IsNullOrEmpty (avatarName)) {
			Debug.LogError ("AnimationsCreator :: CreateAnimationController -- Avatar Name is not set.");
			return;
		}
		CreateNewAnimations ();
		AnimatorState[] animatorStates = 
			new AnimatorState[animations.Length];
		bool hasDefaultState = false;
		AnimatorStateTransition transition;
		// Creates controller.
		var animationController = AnimatorController.CreateAnimatorControllerAtPath (
			                         "Assets/AnimatorController/" + avatarName + ".controller");
		// Add root state machine.
		var rootStateMachine = animationController.layers [0].stateMachine;
		// Add parameters and states and transitions.
		for (int i = 0; i < animations.Length; i++) {
			animationController.AddParameter (animations [i].name, AnimatorControllerParameterType.Trigger);
			animatorStates [i] = rootStateMachine.AddState (animations [i].name);
			animatorStates [i].motion = animationsDictionary [animations [i]];
			transition = rootStateMachine.AddAnyStateTransition (animatorStates [i]);
			transition.duration = 0;
			transition.exitTime = 0;
			transition.AddCondition (AnimatorConditionMode.If, 0, animations [i].name);
			if (animations [i].isDefault && hasDefaultState == false) {
				hasDefaultState = true;
				rootStateMachine.defaultState = animatorStates [i];
			}
		}
		if (hasDefaultState == false)
			rootStateMachine.defaultState = animatorStates [0];
		CreateSpritePrefab (animationController);	
	}
		
	void CreateNewAnimations(){
		AnimationClip newAnimationClip;
		animationsDictionary.Clear ();
		if (AssetDatabase.IsValidFolder ("Assets/AnimatorController/" + avatarName + "Animations") == false)
			AssetDatabase.CreateFolder ("Assets/AnimatorController", avatarName + "Animations");
		foreach(var animation in animations){
			newAnimationClip = CreateAnimation (animation);
			animationsDictionary.Add (animation, newAnimationClip);
		}
	}


	AnimationClip CreateAnimation (AnimationClass animation){
		int framesCount = animation.Lenght;
		AnimationClip newAnimationClip = new AnimationClip ();
		AnimationClipSettings newSettings = new AnimationClipSettings ();
		newSettings.loopTime = animation.isLooping;
		newAnimationClip.frameRate = framesCount * 2;
		AnimationUtility.SetAnimationClipSettings(newAnimationClip, newSettings);
		EditorCurveBinding curveBinding = new EditorCurveBinding ();
		curveBinding.type = typeof(SpriteRenderer);
		curveBinding.path = "";
		curveBinding.propertyName = "m_Sprite";
		ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[framesCount];
		for(int i=0; i<framesCount; i++){
			keyFrames [i] = new ObjectReferenceKeyframe ();
			keyFrames [i].time = (float)i/newAnimationClip.frameRate;
			keyFrames [i].value = sprites [animation.firstFrame+i];
		}
		AnimationUtility.SetObjectReferenceCurve (newAnimationClip, curveBinding, keyFrames);
		AssetDatabase.CreateAsset(newAnimationClip, "Assets/AnimatorController/" + avatarName+ "Animations/"
			+animation.name+".anim");
		AssetDatabase.SaveAssets();

		return newAnimationClip;
	}


	void CreateSpritePrefab(AnimatorController animationController ){
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