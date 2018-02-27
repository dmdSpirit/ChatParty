using UnityEngine;
using System;

namespace dmdSpirit {
    /// <summary>
    /// System component that inits Random and sets window resolution in build version.
    /// </summary>
    public class SystemController : MonoBehaviour {
        void Awake() {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
#if !UNITY_EDITOR
		Screen.SetResolution(1070, 200, false);
		Application.runInBackground = true;
#endif
        }
    }
}
