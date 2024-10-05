using UnityEngine;

public class DaemonEditorUI : MonoBehaviour
{
    public DaemonActionListUI testListUI;

    private DaemonGame game;
    private Daemon lastDaemon;


    private void Start()
    {
        game = FindObjectOfType<DaemonGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game.selectedDaemon != lastDaemon)
        {
            Daemon nextDaemon = game.selectedDaemon;

            if (nextDaemon == null)
            {
                // TODO Clear or gray out?
                testListUI.Clean();
            }
            else
            {
                // Populate the test
                testListUI.Populate(nextDaemon.OnIdle);
            }

            lastDaemon = nextDaemon;
        }
    }
}
