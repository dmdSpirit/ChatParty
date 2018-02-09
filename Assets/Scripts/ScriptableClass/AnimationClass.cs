using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "ScriptableObject/Animation")]
public class AnimationClass : ScriptableObject {
	public new string name;
	public int start;
	public int end;
	public bool isDefault = false;
	public bool isLooping = true;
}
