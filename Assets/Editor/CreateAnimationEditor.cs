using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationsCreator))]
public class CreateAnimationEditor : Editor {
	List<Editor> animationEditors;
	string foldoutText = "Show animations parameters.";
	bool showAnimationParams = false;

	void OnEnable (){
		animationEditors = new List<Editor> ();
	}

	public override void OnInspectorGUI () {

		DrawDefaultInspector ();
		AnimationsCreator script = (AnimationsCreator)target;
		if(script.animations.Length != animationEditors.Count){
			animationEditors.Clear ();
			foreach (var animation in script.animations)
				animationEditors.Add (Editor.CreateEditor (animation));
		}
		showAnimationParams = EditorGUILayout.Foldout (showAnimationParams, foldoutText);
		if (showAnimationParams) {
			foreach (var animationEditor in animationEditors) {
				EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
				animationEditor.DrawDefaultInspector ();
			}
			foldoutText = "Hide animations parameters.";
		} else{
			foldoutText = "Show animations parameters.";
		}
			

		if(GUILayout.Button("Load Sprites")){
			script.LoadSprites ();
		}
		if(GUILayout.Button("Create Sprite Prefab")){
			script.CreateAnimationController ();
		}
	}
}

