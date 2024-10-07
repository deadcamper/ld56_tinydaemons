using System.Collections;
using UnityEngine;

public class DaemonAction_BackAwayFromWall : DaemonAction
{
    public string backawayAnimation = "Idle";

    public float distance = 1;

    public float impulseSpeed = 0.5f;

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        UnityEngine.Animator aa = parentDaemon.body.animator;
        aa.Play(backawayAnimation);

        Collider bump = parentDaemon.lastBumped;
        Vector3 impulse = parentDaemon.lastBumpedImpulse;
        Vector3 knockBack = EvalKnockback(parentDaemon, bump, impulse) * impulseSpeed;

        float time = distance / impulseSpeed;

        Rigidbody arbys = parentDaemon.GetComponent<Rigidbody>();

        arbys.AddForce(knockBack, ForceMode.Impulse);

        yield return new WaitForSeconds(time);

        arbys.velocity = Vector3.zero;

        yield return new WaitForEndOfFrame();
    }

    private Vector3 EvalKnockback(Daemon parentDaemon, Collider bump, Vector3 impulse)
    {
        if (bump.gameObject.CompareTag("Wall"))
        {
            Vector3 knockBack = Vector3.Normalize(impulse);
            return knockBack;
        }

        Vector3 myPos = parentDaemon.transform.position;
        Vector3 theirPos = bump.GetComponent<Collider>().transform.position;

        // zeroing out the floor
        myPos.y = 0;
        theirPos.y = 0;

        Vector3 dir = myPos - theirPos;
        return dir.normalized;
    }
}
