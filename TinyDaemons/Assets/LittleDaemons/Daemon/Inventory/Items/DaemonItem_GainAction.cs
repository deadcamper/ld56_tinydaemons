using UnityEngine;

public class DaemonItem_GainAction : DaemonItem
{
    public DaemonAction gainedAction;

    public override void ApplyItem()
    {
        DaemonGame game = DaemonGame.GetInstance();
        game.selectedListForInventory.actions.Add(gainedAction);
    }

    public override Sprite GetSprite()
    {
        return null;
    }

    public override string GetText()
    {
        return gainedAction.TextName;
    }
}
