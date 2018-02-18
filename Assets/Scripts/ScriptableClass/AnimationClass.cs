using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject to store description of animation.
/// </summary>
[CreateAssetMenu (menuName = "ScriptableObject/Animation")]
public class AnimationClass : ScriptableObject {
	/// <summary>
	/// Name of the animation.
	/// </summary>
	public new string name;
	/// <summary>
	/// Number of the first animation frame in spriteSheet.
	/// </summary>
	public int firstFrame;
	/// <summary>
	/// Number of the last animation frame in spriteSheet.
	/// </summary>
	public int lastFrame;
	/// <summary>
	/// Animation is set as a defaut animation in animator. If there is more 
	/// than 1 animation with this property this behavour is inconsistent.
	/// </summary>
	public bool isDefault = false;
	/// <summary>
	/// Sets animation clip looping property.
	/// </summary>
	public bool isLooping = true;
	/// <summary>
	/// Animation length in frames.
	/// </summary>
	public int Lenght{
		get{
			return lastFrame - firstFrame + 1;
		}
	}
}
