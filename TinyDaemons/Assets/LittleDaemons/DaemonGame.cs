using UnityEngine;

/// <summary>
/// Keeps track of basic game state.
/// </summary>
public class DaemonGame : MonoBehaviour
{
    public Daemon selectedDaemon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
