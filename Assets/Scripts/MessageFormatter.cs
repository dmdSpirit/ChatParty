using System.Collections.Generic;

/// <summary>
/// Singleton system component for splitting long messages.
/// </summary>
public class MessageFormatter : MonoSingleton<MessageFormatter> {
	public int maxCharInLine = 15;
	public int maxLines = 3;

	/// <summary>
	/// Splits message into lines of length maxCharInLine adding '\n'.
	/// </summary>
	/// <returns>Formated message.</returns>
	/// <param name="message">Message.</param>
	public string[] SplitMessageIntoLines(string message){
		if (maxCharInLine == 0) {
			Logger.LogMessage("MessageFormatter :: maxCharInLine is 0.", LogType.Error);
			return null;
		}
        List<string> messageLinesList = new List<string>();
		while( message != ""){
            string newLine;
            int pos;
            message = message.Trim ();
			if (message.Length > maxCharInLine) {
				newLine = message.Substring (0, maxCharInLine+1);
				pos = newLine.LastIndexOf (" ");
				if (pos == -1) {
					messageLinesList.Add(newLine.Substring(0,maxCharInLine));
					message = message.Substring (maxCharInLine);
				} else if(pos == maxCharInLine){
                    messageLinesList.Add(newLine);
					message = message.Substring (maxCharInLine+1);
				}
				else {
                    messageLinesList.Add(newLine.Substring (0, pos));
					message = message.Substring (pos + 1);
				}
			}
			else{
                messageLinesList.Add(message);
				message = "";
			}
		}
		return messageLinesList.ToArray();
	}
}
