using UnityEngine;

public class DaemonItem_GainAction : DaemonItem
{
    public DaemonAction gainedAction;

    public override void ApplyItem()
    {
        throw new System.NotImplementedException();
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
