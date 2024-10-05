using System.Collections;
using UnityEngine;

public class DaemonAction_SwitchState : DaemonAction
{
    public Daemon.DaemonState newState;

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        parentDaemon.activeState = newState;
        yield return new WaitForEndOfFrame();
    }
}
