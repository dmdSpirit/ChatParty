using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandClass : ScriptableObject {
    public string command;

    public abstract void PerformCommand();
}
