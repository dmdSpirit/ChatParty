using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace dmdspirit.newScript {

    public class FightController : MonoBehaviour {
        [SerializeField]
        BrainController[] fighters;
        Dictionary<BrainController, float> turnTimers;
        Dictionary<int, List<BrainController>> teamsDictionary;

        bool takeNewTurn = false;
        Queue<BrainController> firstTurnQueue;

        public event Action<FightController> OnFightEnded;

        public void StartFight(BrainController[] f) {
            fighters = f;
            InitTeams();
            if (teamsDictionary.Count < 2) {
                Debug.Log("Fight could not be started, less than 2 teams.");
                return;
            }
            firstTurnQueue = new Queue<BrainController>();
            turnTimers = new Dictionary<BrainController, float>();
            foreach (var fighter in fighters) {
                float initiative = fighter.StartFight(this);
                turnTimers.Add(fighter, initiative);
            }
            foreach (var brainInitPair in turnTimers.ToList().OrderByDescending(t => t.Value)) {
                firstTurnQueue.Enqueue(brainInitPair.Key);
                turnTimers[brainInitPair.Key] = 0;
            }
            takeNewTurn = true;
        }

        void Update() {
            if (takeNewTurn)
                TakeNewTurn();
        }

        public void TakeNewTurn() {
            takeNewTurn = false;
            BrainController nextFighter;
            // If it is still first turn.
            if (firstTurnQueue.Count != 0) {
                LogFirstTurnQueue();
                nextFighter = firstTurnQueue.Dequeue();
            }
            else {
                LogTimeline();
                nextFighter = turnTimers.ToList().OrderBy(t => t.Value).First().Key;
                foreach (var fighter in fighters)
                    turnTimers[fighter] -= turnTimers[nextFighter];
            }
            Debug.Log(nextFighter.gameObject.name + " now attacks");
            nextFighter.StartTurn();
        }

        public void EndTurn(BrainController lastFighter, float cooldown) {
            turnTimers[lastFighter] = cooldown;
            takeNewTurn = true;
        }

        public BrainController GetRandomEnemy(BrainController brainController) {
            List<BrainController> possibleEnemies = GetPossibleEnemiesList(brainController);
            int enemyID = UnityEngine.Random.Range(0, possibleEnemies.Count);
            BrainController enemy = possibleEnemies.ToArray()[enemyID];
            Debug.Log(brainController.gameObject.name + " chooses to attack " + enemy.gameObject.name);

            return enemy;
        }

        public BrainController GetClosestEnemy(BrainController brainController) {
            //Dictionary<BrainController, float> enemyRangeDictionary = new Dictionary<BrainController, float>();
            float nearestRange = Mathf.Infinity;
            BrainController closestEnemy = null;
            foreach (var enemy in GetPossibleEnemiesList(brainController)) {
                var range = Mathf.Abs(enemy.transform.position.x - brainController.transform.position.x);
                if (range < nearestRange) {
                    nearestRange = range;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        private List<BrainController> GetPossibleEnemiesList(BrainController brainController) {
            List<BrainController> possibleEnemies = new List<BrainController>();
            int fighterTeamID = GetFighterTeamID(brainController);
            foreach (var teamID in teamsDictionary.Keys)
                if (teamID != fighterTeamID)
                    possibleEnemies.AddRange(teamsDictionary[teamID]);
            return possibleEnemies;
        }

        private int GetFighterTeamID(BrainController brainController) {
            foreach (var team in teamsDictionary)
                foreach (var fighter in team.Value)
                    if (fighter == brainController)
                        return team.Key;
            return 0;
        }

        public void InitTeams() {
            if (teamsDictionary == null)
                teamsDictionary = new Dictionary<int, List<BrainController>>();
            else
                teamsDictionary.Clear();
            foreach (var fighter in fighters) {
                int teamID = UnityEngine.Random.Range(1, fighters.Length + 1);
                if (teamsDictionary.ContainsKey(teamID) == false) {
                    teamsDictionary[teamID] = new List<BrainController>();
                }
                teamsDictionary[teamID].Add(fighter);
            }
            LogTeamDictionary();
        }

        [ContextMenu("Start fight")]
        public void Test() {
            StartFight(fighters);
        }



        public void LogTimeline() {
            string timeline = "";
            foreach (var t in turnTimers.ToList().OrderBy(t => t.Value)) {
                timeline += string.Format("{0}: {1}, ", t.Key.gameObject.name, t.Value);
            }
            Debug.Log(timeline);
        }

        public void LogFirstTurnQueue() {
            string timeline = "First turn fighters left: ";
            foreach (var t in firstTurnQueue) {
                timeline += t.gameObject.name + ", ";
            }
            Debug.Log(timeline);
        }

        public void LogTeamDictionary() {
            string teamString;
            foreach (var teamID in teamsDictionary.Keys) {
                teamString = "";
                foreach (var fighter in teamsDictionary[teamID])
                    teamString += ", " + fighter.gameObject.name;
                Debug.Log("Team " + teamID + ": " + teamString);
            }
        }

        //// ------------- old
        public void OnTurnEnded(BrainController lastFighter) {
            //turnTimers[lastFighter] = lastFighter.turnTimer;
            takeNewTurn = true;
        }
    }

}
