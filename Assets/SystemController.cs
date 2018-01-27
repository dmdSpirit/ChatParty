using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemController : MonoBehaviour {
	void Awake(){
#if UNITY_STANDALONE_WIN
		Screen.SetResolution(1070, 200, false);
		Application.runInBackground = true;
#endif    
	}
}
