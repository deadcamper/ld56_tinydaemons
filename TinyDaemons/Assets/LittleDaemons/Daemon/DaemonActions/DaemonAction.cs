using System.Collections;
using UnityEngine;

public abstract class DaemonAction : MonoBehaviour
{
    public string TextName; // Used for presentation

    [SerializeField]
    public bool IsPerforming { get; private set; }

    public IEnumerator PerformAction(Daemon parentDaemon)
    {
        IsPerforming = true;
        yield return InternalAction(parentDaemon);
        IsPerforming = false;
    }

    protected abstract IEnumerator InternalAction(Daemon parentDaemon);
}
