using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace dmdSpirit {
    /// <summary>
    /// Combat controlling component.
    /// </summary>
    public class FightController : MonoBehaviour {
        public bool isFighting = false;

        [SerializeField]
        BrainController[] fighters;
        [SerializeField]
        StartMassFightCommand massFightCommand;

        BrainController[] initialFightersList;

        Dictionary<BrainController, float> turnTimersDictionary;
        Dictionary<BrainController, float> initiativeDictionary;
        Dictionary<int, List<BrainController>> teamsDictionary;
        Queue<BrainController> turnQueue;
        bool takeNewTurn = false;
        bool fightEnded = false;
        bool startFight = false;

        private void Awake() {
            // HACK: Fix this after creating more than one fightController.
            massFightCommand.fightController = this;
        }

        void Update() {
            if (startFight) {
                StartFight(fighters);
                startFight = false;
            }
            else if (isFighting) {
                if (takeNewTurn) {
                    if (fightEnded)
                        EndFight();
                    else
                        TakeNewTurn();
                }
            }
        }

        /// <summary>
        /// End turn for current fighter and passes turn to next one.
        /// </summary>
        /// <param name="lastFighter">Current fighter.</param>
        /// <param name="cooldown">Next ability cooldown.</param>
        public void EndTurn(BrainController lastFighter, float cooldown) {
            turnTimersDictionary[lastFighter] = cooldown;
            takeNewTurn = true;
        }

        /// <summary>
        /// Gets random brainController from opposing team.
        /// </summary>
        /// <param name="brainController">Current fighter.</param>
        /// <returns>Random enemy.</returns>
        public BrainController GetRandomEnemy(BrainController brainController) {
            List<BrainController> possibleEnemies = GetPossibleEnemiesList(brainController);
            int enemyID = UnityEngine.Random.Range(0, possibleEnemies.Count);
            BrainController enemy = possibleEnemies.ToArray()[enemyID];
            Debug.Log(brainController.gameObject.name + " chooses to attack " + enemy.gameObject.name);
            return enemy;
        }

        /// <summary>
        /// Gets closest enemy brainController.
        /// </summary>
        /// <param name="brainController">Current fighter.</param>
        /// <returns>Closest enemy.</returns>
        public BrainController GetClosestEnemy(BrainController brainController) {
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

        public void StartFight(BrainController[] f) {
            fightEnded = false;

            fighters = f;
            initialFightersList = (BrainController[])f.Clone();
            InitTeams();
            if (teamsDictionary.Count < 2) {
                Debug.Log("Fight could not be started, less than 2 teams.");
                return;
            }
            turnQueue = new Queue<BrainController>();
            turnTimersDictionary = new Dictionary<BrainController, float>();
            initiativeDictionary = new Dictionary<BrainController, float>();
            foreach (var fighter in fighters) {
                float initiative = fighter.StartFight(this, GetFighterTeamID(fighter));
                turnTimersDictionary.Add(fighter, initiative);
                initiativeDictionary.Add(fighter, initiative);
            }
            foreach (var brainInitPair in turnTimersDictionary.ToList().OrderByDescending(t => t.Value)) {
                turnQueue.Enqueue(brainInitPair.Key);
                turnTimersDictionary[brainInitPair.Key] = 0;
            }
            takeNewTurn = true;
            isFighting = true;
        }

        private void InitTeams() {
            teamsDictionary = new Dictionary<int, List<BrainController>>();
            foreach (var fighter in fighters) {
                int teamID = UnityEngine.Random.Range(1, fighters.Length + 1);
                if (teamsDictionary.ContainsKey(teamID) == false) {
                    teamsDictionary[teamID] = new List<BrainController>();
                }
                teamsDictionary[teamID].Add(fighter);
            }
        }

        private void TakeNewTurn() {
            takeNewTurn = false;
            BrainController nextFighter;
            // If it is still first turn or 
            if (turnQueue.Count != 0) {
                nextFighter = turnQueue.Dequeue();
            }
            else {
                var timersList = turnTimersDictionary.ToList();
                // If several fighters have same cooldown we store them in queue ordering by initiative.
                float minTimer = timersList.Min(t => t.Value);
                var nextFightersList = timersList.Where(t => t.Value == minTimer);
                float nextFighterCooldown = turnTimersDictionary[nextFightersList.First().Key];
                foreach (var fighter in fighters)
                    turnTimersDictionary[fighter] -= nextFighterCooldown;
                if (nextFightersList.Count() != 1) {
                    turnQueue.Clear();
                    Dictionary<BrainController, float> nextFightersDictionary = new Dictionary<BrainController, float>();
                    foreach (var fighter in nextFightersList)
                        nextFightersDictionary.Add(fighter.Key, initiativeDictionary[fighter.Key]);
                    var nextFightersQueue = nextFightersDictionary.ToList().OrderBy(t => t.Value);
                    foreach (var nF in nextFightersQueue)
                        turnQueue.Enqueue(nF.Key);
                    nextFighter = turnQueue.Dequeue();
                }
                else {
                    nextFighter = nextFightersList.First().Key;
                }
            }
            Debug.Log(nextFighter.gameObject.name + " now attacks");
            nextFighter.StartTurn();
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


        public void OnDeath(BrainController deadFighter) {
            int teamID = GetFighterTeamID(deadFighter);
            if(turnQueue.Contains(deadFighter))
                turnQueue = new Queue<BrainController>(turnQueue.Where(t => t != deadFighter));
            turnTimersDictionary.Remove(deadFighter);
            Logger.LogMessage($"FightController::{deadFighter.gameObject.name} removed from turnTimersDictionary");
            fighters = turnTimersDictionary.Keys.ToArray();
            initiativeDictionary.Remove(deadFighter);
            Logger.LogMessage($"FightController::{deadFighter.gameObject.name} removed from initiativeDictionary");
            teamsDictionary[teamID].Remove(deadFighter);
            Logger.LogMessage($"FightController::{deadFighter.gameObject.name} removed from teamsDictionary");
            // FIXME: DictionaryKeyNotFound excepntion beggining from second combat.
            if (teamsDictionary[teamID].Count <= 0) {
                teamsDictionary.Remove(teamID);
                if (teamsDictionary.Count <= 1)
                    fightEnded = true;
            }
        }

        private void EndFight() {
            foreach (var fighter in initialFightersList)
                fighter.EndFight();
            isFighting = false;
        }

        [ContextMenu("Start fight")]
        public void Test() {
            //StartFight(fighters);
            startFight = true;
        }
    }
}