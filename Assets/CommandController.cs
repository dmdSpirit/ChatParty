using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dmdSpirit;

public class CommandController : MonoSingleton<CommandController> {
    [SerializeField]
    CommandClass[] commandsArray;

    Queue<CommandClass> commandsQueue;

    private void Awake() {
        commandsQueue = new Queue<CommandClass>();
        CheckIsSingleInScene();
    }
	
	// Update is called once per frame
	void Update () {
        if (commandsQueue.Count != 0)
            commandsQueue.Dequeue().PerformCommand();
	}

    public void AddCommand(string command) {
        foreach (var c in commandsArray) 
            if (c.command == command)
                commandsQueue.Enqueue(c);
    }
}
