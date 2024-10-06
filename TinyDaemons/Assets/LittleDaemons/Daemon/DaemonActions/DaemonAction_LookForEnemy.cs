using System.Collections;
using UnityEngine;

public class DaemonAction_LookForEnemy : DaemonAction
{
    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        yield return new WaitForSeconds(0.25f); // Because this is quick otherwise
        if (parentDaemon.enemy)
        {
            if (parentDaemon.enemy.Health < 0)
                parentDaemon.enemy = null;
            else
                parentDaemon.Interrupt(Daemon.InterruptState.EnemySpotted);
                yield break; // TODO signal?
        }

        Daemon[] daemons = FindObjectsOfType<Daemon>();

        Daemon foundEnemy=null;

        foreach (Daemon dd in daemons)
        {
            if (dd.Health <= 0)
                continue;

            if ( ! parentDaemon.enemyTypes.Contains(dd.daemonType))
                continue;

            // TODO - Anything about line of sight? It seems a moot point at this stage...

            foundEnemy = dd;
            break;
        }

        if (foundEnemy)
        {
            parentDaemon.enemy = foundEnemy;
            parentDaemon.Interrupt(Daemon.InterruptState.EnemySpotted);
        }

    }
}
