using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector extension for AvatarStats component. Shows in the inspector data from AvatarStats scriptableObject.
/// </summary>
[CustomEditor(typeof(AvatarStats))]
public class AvatarStatsEditor : Editor {
    string foldoutText = "Show AvatarStats object data.";
    bool showAvatarStatsData = false;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        AvatarStats avatarStats = (AvatarStats)target;
        if(avatarStats.stats != null) {
            showAvatarStatsData = EditorGUILayout.Foldout(showAvatarStatsData, foldoutText);
            if (showAvatarStatsData) {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                Editor statsEditor = Editor.CreateEditor(avatarStats.stats);
                statsEditor.DrawDefaultInspector();
                foldoutText = "Hide AvatarStats object data.";
            }
            else
                foldoutText = "Show AvatarStats object data.";

        }

    }
}
