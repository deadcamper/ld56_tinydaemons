using System.Collections;
using UnityEngine;

public class DaemonAction_Throw : DaemonAction
{
    public DaemonProjectile fireballTemplate;

    public string throwAnimation = "AttackThrow";

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        UnityEngine.Animator aa = parentDaemon.body.animator;
        aa.Play(throwAnimation);

        // Play animation in full
        yield return new WaitForEndOfFrame();
        while (true)
        {
            var curState = aa.GetCurrentAnimatorStateInfo(0);

            if (!curState.IsName(throwAnimation))
            {
                break;
            }
            if (1 <= curState.normalizedTime)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        // Get direction to throw
        Vector3 throwDir;
        if (parentDaemon.enemy)
        {
            throwDir = parentDaemon.enemy.transform.position - parentDaemon.transform.position;
        }
        else
        {
            throwDir = parentDaemon.body.transform.forward;
        }

        DaemonProjectile newProjectile = Instantiate(fireballTemplate, parentDaemon.transform.position, Quaternion.identity);
        newProjectile.Launch(parentDaemon, throwDir);
    }
}
