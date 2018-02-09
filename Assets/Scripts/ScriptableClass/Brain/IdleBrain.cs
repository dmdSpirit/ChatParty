using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Brains/IdleBrain")]
public class IdleBrain : Brain{
	public SimpleBehaviourClass[] behaviours;

	int totalBehaviourWeight;
	int behavioursLength;

	void Awake(){
		foreach (var behavoiur in behaviours)
			behavoiur.BehaviourEnded += OnBehaviourEnded;
	}

	void OnValidate(){
		foreach (var behavoiur in behaviours)
			behavoiur.BehaviourEnded += OnBehaviourEnded;
	}

	public override BehaviourClass InitBrain(BrainController brainController){
		return GetRandomBehaviour ();
	}

	public void OnBehaviourEnded(BrainController brainController){
		BehaviourClass newBehaviour = GetRandomBehaviour ();
		brainController.ChangeBehaviour (newBehaviour);
	}

	public BehaviourClass GetRandomBehaviour(){
		if (behavioursLength != behaviours.Length)
			CalculateTotalWeight ();
		if (behaviours.Length != 0) {
			float t = Random.Range (0, totalBehaviourWeight);
			int newBehaviour=0;
			int currentWeight=0;
			for (int i = 0; i < behaviours.Length; i++) {
				if (t < currentWeight + behaviours [i].weight) {
					newBehaviour = i;
					break;
				}
				currentWeight += behaviours [i].weight;
			}
			return (BehaviourClass)behaviours [newBehaviour];
		}
		else{
			return null;
		}
	}

	void CalculateTotalWeight(){
		totalBehaviourWeight = 0;
		foreach (var behaviour in behaviours)
			totalBehaviourWeight += behaviour.weight;
		behavioursLength = behaviours.Length;
	}
}