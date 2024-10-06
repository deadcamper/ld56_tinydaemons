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

    internal DaemonActionList selectedListForInventory;

    internal DaemonItem selectedItem;


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

    public void DeselectSelf(Daemon self)
    {
        if (self == selectedDaemon)
        {
            selectedDaemon = null;
        }
    }

    public void OnPickupGib()
    {
        DaemonItem prefab = itemStore.RandomItem();
        if (prefab)
        {
            GainItem(prefab);
        }
    }

    public bool GainItem(DaemonItem prefab)
    {
        if (inventory.AtMaxCapacity())
        {
            return false;
        }

        inventory.AddItem(prefab);
        return true;
    }
}
