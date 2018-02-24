using UnityEngine;
 
/// <summary>
/// Component responsible for handling Avatar stats.
/// </summary>
public class AvatarStats : MonoBehaviour {
	public AvatarStatsClass stats;

    public void LoadStats(string avatarName) {
        stats = Resources.Load<AvatarStatsClass>($"AvatarStats/{avatarName}");
    }

}
