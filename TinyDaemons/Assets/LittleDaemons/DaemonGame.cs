using UnityEngine;

/// <summary>
/// Keeps track of basic game state.
/// </summary>
public class DaemonGame : MonoBehaviour
{
    public Material defaultHover;
    public DaemonInventory inventory;

    public DaemonInventory itemStore;

    public Daemon selectedDaemon { get; private set; }

    public DaemonActionList selectedList { get; private set; }


    private static DaemonGame _singleton;
    public static DaemonGame GetInstance()
    {
        if (!_singleton)
        {
            _singleton = FindObjectOfType<DaemonGame>();
        }

        return _singleton;
    }

    public void SelectDaemon(Daemon daemon)
    {
        var lastSelectedDaemon = selectedDaemon;
        if (selectedDaemon)
        {
            selectedDaemon.selectionCircle.DeSelect();
            selectedDaemon = null;
        }

        // This is deselect
        if (lastSelectedDaemon == daemon)
            return;

        daemon.selectionCircle.Select();
        selectedDaemon = daemon;
    }

    public void OnPickupGib()
    {
        DaemonItem item = itemStore.RandomItem();
        if (item)
        {
            GainItem(item);
        }
    }

    public bool GainItem(DaemonItem item)
    {
        if (inventory.AtMaxCapacity())
        {
            return false;
        }

        inventory.items.Add(item);
        return true;
    }
}
