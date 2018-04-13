using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace dmdSpirit {
    [CustomEditor(typeof(AddEventToClip))]
    public class AddEventToClipEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            AddEventToClip addEventToClip = (AddEventToClip)target;
            if (GUILayout.Button("Add Event.")) {
                addEventToClip.AddEventToEnd();
            }
        }
    }
}
