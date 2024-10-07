using System.Collections;
using UnityEngine;

public class DaemonAction_SwitchState : DaemonAction
{
    public Daemon.DaemonState newState;

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        yield return new WaitForSeconds(0.125f); // To avoid flicker
        parentDaemon.activeState = newState;
    }
}
