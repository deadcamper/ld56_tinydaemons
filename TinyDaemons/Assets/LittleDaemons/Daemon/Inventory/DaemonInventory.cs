using System.Collections.Generic;
using UnityEngine;

public class DaemonInventory : MonoBehaviour
{
    public int MaxCount = 4;
    public List<DaemonItem> items;

    public bool AtMaxCapacity()
    {
        return items.Count >= MaxCount;
    }

    public void AddItem(DaemonItem prefab)
    {
        DaemonItem item = Instantiate(prefab);
        items.Add(item);
    }

    public DaemonItem RandomItem()
    {
        if (items.Count == 0)
            return null;
        return items[Random.Range(0, items.Count)];
    }

}
