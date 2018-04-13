using UnityEngine;
using dmdSpirit;

[CreateAssetMenu(menuName = "ScriptableObject/TurnInto/TurnIntoDefault", fileName = "TurnInto")]
public class TurnIntoClass : ScriptableObject {
    public string spriteName;
    public IdleBrain idleBrain;
    public FightBrain fightBrain;
}
