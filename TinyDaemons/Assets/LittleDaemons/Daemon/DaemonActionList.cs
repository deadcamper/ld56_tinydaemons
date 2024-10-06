using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class DaemonActionList
{
    public string title;
    public List<DaemonAction> actions;

    private int performingActionIndex = -1;
    private bool isPerforming;

    public bool IsPerforming()
    {
        return isPerforming;
    }

    public void NotPerforming()
    {
        isPerforming = false;
    }

    public int GetPerformingIndex()
    {
        if (!isPerforming)
        {
            return -1;
        }
        return performingActionIndex;
    }

    public IEnumerator DoListOfActions(Daemon parentDaemon)
    {
        isPerforming = true;
        performingActionIndex = 0;
        foreach (DaemonAction action in actions)
        {
            yield return action.PerformAction(parentDaemon);
            performingActionIndex++;
        }
        isPerforming = false;
    }
}
