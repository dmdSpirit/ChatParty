using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AttackClass : ScriptableObject {
	public string animationTrigger;
	public int weight;
	public float cooldown;

	public abstract void Attack(BrainController brainController);
}
