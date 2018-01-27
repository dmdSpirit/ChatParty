using UnityEngine;

public abstract class BehaviorClass : ScriptableObject {
	public abstract void Act (AvatarController avatarController);
	public abstract void Init(AvatarController avatarController);
}
