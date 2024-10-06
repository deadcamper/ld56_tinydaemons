using UnityEngine;

/// <summary>
/// Keeps track of basic game state.
/// </summary>
public class DaemonGame : MonoBehaviour
{
    public Material defaultHover;

    public Daemon selectedDaemon { get; private set; }


    private static DaemonGame _singleton;
    public static DaemonGame GetInstance()
    {
        if (!_singleton)
        {
            _singleton = FindObjectOfType<DaemonGame>();
        }

        return _singleton;
    }

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

    public void OnPickupGib()
    {

    }
}
