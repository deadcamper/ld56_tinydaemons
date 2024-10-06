using UnityEngine;

public class KillButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void Kill()
    {
        var game = Object.FindAnyObjectByType<DaemonGame>();

        Daemon selectedDaemon = game.selectedDaemon;
        if (selectedDaemon)
        {
            selectedDaemon.Hurt(selectedDaemon.Health);
        }
    }
}
