using UnityEngine;

/// <summary>
/// Singleton system component for splitting long messages.
/// </summary>
public class MessageFormatter : MonoSingleton<MessageFormatter> {
	public int maxCharInLine = 15;
	public int maxLines = 3;

	string formatedMessage;

	/// <summary>
	/// Splits message into lines of length maxCharInLine adding '\n'.
	/// </summary>
	/// <returns>Formated message.</returns>
	/// <param name="message">Message.</param>
	public string FormatMessage(string message){
		if (maxCharInLine == 0) {
			Debug.LogError("MessageFormatter :: maxCharInLine is 0.");
			return message;
		}
		string newLine;
		int pos;
		formatedMessage = "";
		while( message != ""){
			message = message.Trim ();
			if (message.Length > maxCharInLine) {
				newLine = message.Substring (0, maxCharInLine+1);
				pos = newLine.LastIndexOf (" ");
				if (pos == -1) {
					formatedMessage += newLine.Substring(0,maxCharInLine) + "\n";
					message = message.Substring (maxCharInLine);
				} else if(pos == maxCharInLine){
					formatedMessage += newLine + "\n";
					message = message.Substring (maxCharInLine+1);
				}
				else {
					formatedMessage += newLine.Substring (0, pos) + "\n";
					message = message.Substring (pos + 1);
				}
			}
			else{
				formatedMessage += message;
				message = "";
			}
		}
		return formatedMessage;
	}
		
}
