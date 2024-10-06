using System.Collections.Generic;
using UnityEngine;

public class DaemonInventory : MonoBehaviour
{
    public int MaxCount = 4;

    public List<DaemonItem> items;

    public DaemonItem RandomItem()
    {
        if (items.Count == 0)
            return null;
        return items[Random.Range(0, items.Count)];
    }

    public bool AtMaxCapacity() { return items.Count >= MaxCount; }
}
