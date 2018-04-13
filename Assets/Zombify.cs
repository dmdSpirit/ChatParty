using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dmdSpirit;

public class Zombify : MonoBehaviour {
    public float zombifyTime;
    public float zombifyChance;
    public float zombifyTickTime;
    public float firstMessageDelay;

    [SerializeField]
    TurnIntoClass zombieScriptable;

    float lastMessageTime = 0;
    float lastTickTime = 0;
    bool checkForZombify = true;

    AvatarController avatarController;

    private void Awake() {
        avatarController = GetComponent<AvatarController>();
    }

    private void Update() {
        if (checkForZombify && avatarController.InCombat == false) {
            float currentTime = Time.realtimeSinceStartup;
            if ((lastTickTime != 0 && currentTime >= lastTickTime + zombifyTickTime) ||
                (lastTickTime == 0 && currentTime - lastMessageTime >= zombifyTime + (lastMessageTime == 0 ? firstMessageDelay : 0))) {
                lastTickTime = currentTime;
                if (zombifyChance >= Random.value)
                    DoZombify();
            }
        }
    }

    void DoZombify() {
        checkForZombify = false;
        lastMessageTime = 0;
        lastTickTime = 0;

        if(zombieScriptable != null)
            avatarController.TurnInto(zombieScriptable);
        // TODO: Change avatar and brain.
    }

    public void NewMessage() {
        lastMessageTime = Time.realtimeSinceStartup;
        lastTickTime = 0;
    }
}
