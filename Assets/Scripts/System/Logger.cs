using UnityEngine;
using System;
using System.IO;

public enum LogType {Message, Warning, Error};

/// <summary>
/// Component for displaying logs depending on platform (editor/windows).
/// </summary>
public static class Logger{
#if !UNITY_EDITOR
	string path;

	static Logger(){
		path = Application.dataPath + "/" + "Log";
	}

	public static void LogMessage(string message, LogType logType = LogType.Message){
	if(String.IsNullOrEmpty(path)== false)
			File.AppendAllText(path, string.Format("{0}:{1}  :: {2} \n", 
				DateTime.Now.ToString("MM/dd/yyyy HH:mm"), logType.ToString(), message));
	}
#elif UNITY_EDITOR
	/// <summary>
	/// Dispays message, depending on platform: editor - default Debug.Log, windows - log file.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name = "logType">Type of message logged.</param>
	public static void LogMessage(string message, LogType logType = LogType.Message){
		switch (logType){
		case LogType.Message:
			Debug.Log (message);
			break;
		case LogType.Warning:
			Debug.LogWarning (message);
			break;
		case LogType.Error:
			Debug.LogError (message);
			break;
		}
	}
#endif
}
