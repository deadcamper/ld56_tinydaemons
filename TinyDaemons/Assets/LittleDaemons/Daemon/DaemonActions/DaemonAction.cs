using System.Collections;
using UnityEngine;

public abstract class DaemonAction : MonoBehaviour
{
    public string TextName; // Used for presentation

    public IEnumerator PerformAction(Daemon parentDaemon)
    {
        yield return InternalAction(parentDaemon);
    }

    protected abstract IEnumerator InternalAction(Daemon parentDaemon);
}
