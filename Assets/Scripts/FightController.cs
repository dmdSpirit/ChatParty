using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FightController : MonoBehaviour {
	[SerializeField]
	BrainController[] fighters;
	Dictionary<BrainController, float> turnTimers;
	Dictionary<int, List<BrainController>> teamsDictionary;

	bool takeNewTurn = false;


	Queue<BrainController> firstTurnQueue;


	public event Action<FightController> OnFightEnded;

	public void StartFight(BrainController[] f){
		fighters = f;
		InitTeams ();
		if(teamsDictionary.Keys.Count<2){
			Debug.Log ("Fight could not be started, less than 2 teams.");
			return;
		}
		firstTurnQueue = new Queue<BrainController> ();
		turnTimers = new Dictionary<BrainController, float> ();
		foreach(var fighter in fighters){
			turnTimers.Add (fighter, fighter.GetInitiative ());
			fighter.OnTurnEnded += OnTurnEnded;
			fighter.InitFightingBrain ();
		}
		foreach(var brainInitPair in turnTimers.ToList ().OrderByDescending (t => t.Value)){
			firstTurnQueue.Enqueue (brainInitPair.Key);
			turnTimers [brainInitPair.Key] = 0;
		}
		takeNewTurn = true;
	}

	void Update(){
		if(takeNewTurn)
			TakeNewTurn ();
	}

	public void TakeNewTurn(){
		takeNewTurn = false;
		BrainController nextFighter;
		// If it is still first turn.
		if (firstTurnQueue.Count != 0) {
			LogFirstTurnQueue ();
			nextFighter = firstTurnQueue.Dequeue ();
		}
		else{
			LogTimeline ();
			nextFighter = turnTimers.ToList ().OrderBy (t => t.Value).First ().Key;
			foreach (var fighter in fighters) 
				turnTimers [fighter] -= nextFighter.turnTimer;
		}
		Debug.Log (nextFighter.gameObject.name + " now attacks");
		nextFighter.enemyBrainController = GetEnemy(nextFighter);
		nextFighter.TakeTurn ();

	}

	public void OnTurnEnded(BrainController lastFighter){
		turnTimers [lastFighter] = lastFighter.turnTimer;
		takeNewTurn = true;
	}

	public BrainController GetEnemy(BrainController brainController){
		List<BrainController> possibleEnemies = new List<BrainController> ();
		foreach(var teamID in teamsDictionary.Keys){
			if(teamID != brainController.teamID){
				possibleEnemies.AddRange (teamsDictionary [teamID]);
			}
		}
		int enemyID = UnityEngine.Random.Range (0, possibleEnemies.Count);
		BrainController enemy = possibleEnemies.ToArray () [enemyID];
		Debug.Log (brainController.gameObject.name + " chooses to attack " + enemy.gameObject.name);

		return enemy;
	}

	[ContextMenu("Start fight")]
	public void Test(){
		// TODO: teamID UI.
		foreach (var fighter in fighters) {
			fighter.teamID = UnityEngine.Random.Range (1, fighters.Length + 1);

		}
		StartFight (fighters);
	}

	public void InitTeams(){
		if (teamsDictionary == null)
			teamsDictionary = new Dictionary<int, List<BrainController>> ();
		else
			teamsDictionary.Clear ();
		foreach(var fighter in fighters){
			int teamID = fighter.teamID;
			if(teamsDictionary.ContainsKey(teamID) == false){
				teamsDictionary [teamID] = new List<BrainController> ();
			}
			teamsDictionary [teamID].Add (fighter);
		}
		LogTeamDictionary ();
	}


	public void LogTimeline(){
		string timeline = "";
		foreach( var t in turnTimers.ToList().OrderBy(t => t.Value)){
			timeline += string.Format ("{0}: {1}, ", t.Key.gameObject.name, t.Value);
		}
		Debug.Log (timeline);
	}

	public void LogFirstTurnQueue(){
		string timeline = "First turn fighters left: ";
		foreach( var t in firstTurnQueue){
			timeline += t.gameObject.name + ", ";
		}
		Debug.Log (timeline);
	}

	public void LogTeamDictionary(){
		string teamString;
		foreach(var teamID in teamsDictionary.Keys){
			teamString = "";
			foreach (var fighter in teamsDictionary[teamID])
				teamString += ", " + fighter.gameObject.name;
			Debug.Log ("Team " + teamID + ": " + teamString);
		}
	}
}
