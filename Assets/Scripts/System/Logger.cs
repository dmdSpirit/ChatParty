using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using UnityEngine;

public class Logger: MonoSingleton<Logger> {
	public string fileName;

	string path;

	public void Start(){
#if UNITY_STANDALONE_WIN
		if(String.IsNullOrEmpty(fileName)== false){
			path = Application.streamingAssetsPath + "/" + fileName;
		}
#endif
	}
	
	public void LogMessage(string message){
#if UNITY_STANDALONE_WIN
		if(String.IsNullOrEmpty(path)== false)
			File.AppendAllText(path, string.Format("{0} :: {1} \n", DateTime.Now.ToString("MM/dd/yyyy HH:mm"), message));
#endif
#if UNITY_EDITOR
		Debug.Log(message);
#endif		
	}
}
