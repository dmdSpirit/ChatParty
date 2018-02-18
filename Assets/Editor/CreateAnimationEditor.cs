using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Inspector extension for AnimationsCreator component.
/// </summary>
[CustomEditor(typeof(AnimationsCreator))]
public class CreateAnimationEditor : Editor {
	string foldoutText = "Show animations parameters.";
	bool showAnimationParams = false;
	List<Editor> animationEditors;

	void OnEnable (){
		animationEditors = new List<Editor> ();
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();
		AnimationsCreator animationsCreator = (AnimationsCreator)target;
		if(animationsCreator.animations.Length != animationEditors.Count){
			animationEditors.Clear ();
			foreach (var animation in animationsCreator.animations)
				animationEditors.Add (Editor.CreateEditor (animation));
		}
		showAnimationParams = EditorGUILayout.Foldout (showAnimationParams, foldoutText);
		if (showAnimationParams) {
			foreach (var animationEditor in animationEditors) {
				EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
				animationEditor.DrawDefaultInspector ();
			}
			foldoutText = "Hide animations parameters.";
		} 
		else
			foldoutText = "Show animations parameters.";

		if(GUILayout.Button("Load Sprites"))
			animationsCreator.LoadSprites ();
		if(GUILayout.Button("Create Sprite Prefab"))
			animationsCreator.CreateAnimationController ();
	}
}

