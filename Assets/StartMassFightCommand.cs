using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static dmdSpirit.Logger;
using dmdSpirit;

[CreateAssetMenu(menuName = "ScriptableObject/Commands/StartMassFight")]
public class StartMassFightCommand : CommandClass {
    public FightController fightController;

    public override void PerformCommand() {
        if (fightController == null)
            LogMessage($"{name}::PerformCommand -- fightController is not set.", dmdSpirit.LogType.Error);
        if (fightController.isFighting)
            return;
        var fightersList = AvatarListController.Instance.GetAllFighters();
        fightController.StartFight(fightersList.ToArray());
    }
}
