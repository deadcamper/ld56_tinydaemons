using System.Collections;

public class DaemonAction_BecomeGibs : DaemonAction
{
    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        parentDaemon.body.ExplodeToGibs();
        Destroy(parentDaemon.gameObject);
        yield return null;
    }
}
