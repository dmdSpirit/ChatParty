using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SystemController : MonoBehaviour {
	void Awake(){
		UnityEngine.Random.InitState (DateTime.Now.Millisecond);
#if UNITY_STANDALONE_WIN
		Screen.SetResolution(1070, 200, false);
		Application.runInBackground = true;
#endif    
	}
}
