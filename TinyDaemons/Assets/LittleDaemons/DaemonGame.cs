using System;
using UnityEngine;

/// <summary>
/// Keeps track of basic game state.
/// </summary>
public class DaemonGame : MonoBehaviour
{
    [Serializable]
    public class LearningStages
    {
        public bool HasClickedDaemon;
        public bool HasRemovedAction;
        public bool HasClickedActionFromStore;
        public bool HasAddedAction;
        public bool HasGibletAppeared;
        public bool HasCollectedGiblet;
    }

    internal LearningStages myLearning = new LearningStages();

    public Material defaultHover;
    public DaemonInventory inventory;

    public DaemonItemStore itemStore;

    public DaemonSpawner spawner;
    public Daemon selectedDaemon { get; private set; }

    internal DaemonActionList selectedListForInventory;

    internal DaemonItem selectedItem;

    internal DaemonItemCost selectedItemCost;

    public int gibletPoints { get; private set; }


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
        gibletPoints++;
        /*
        DaemonItem prefab = itemStore.RandomItem();
        if (prefab)
        {
            GainItem(prefab);
        }
        */
        myLearning.HasCollectedGiblet = true;
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
