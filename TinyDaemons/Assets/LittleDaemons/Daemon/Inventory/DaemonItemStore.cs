using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DaemonItemCost
{
    public DaemonAction action;
    public int cost;
    public void BuyItem()
    {
        DaemonGame game = DaemonGame.GetInstance();
        game.selectedListForInventory.actions.Add(action);
    }
}

public class DaemonItemStore : MonoBehaviour
{
    public List<DaemonItemCost> itemCosts = new List<DaemonItemCost>();
}
