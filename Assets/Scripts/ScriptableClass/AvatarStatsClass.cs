using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "ScriptableObject/ViewerStats")]
public class AvatarStatsClass : ScriptableObject {
	public Vector2 walkSpeed;
	public Vector2 runSpeed;
	public Vector2 walkTravelDistance;
	public Vector2 runTravelDistance;
	public Vector2 idleTime;
}
