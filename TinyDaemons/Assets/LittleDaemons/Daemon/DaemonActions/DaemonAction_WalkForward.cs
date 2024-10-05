using System.Collections;
using UnityEngine;

public class DaemonAction_WalkForward : DaemonAction
{
    public string stopAnimation = "Idle";
    public string walkAnimation = "Walk";
    public float maximumTime = 0f;

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        UnityEngine.Animator aa = parentDaemon.body.animator;

        aa.Play(walkAnimation);
        Rigidbody rbody = parentDaemon.GetComponent<Rigidbody>();

        float time = 0;
        while (maximumTime <= 0 || time < maximumTime)
        {
            time += Time.deltaTime;
            Vector3 forward = parentDaemon.body.transform.TransformDirection(Vector3.forward);

            rbody.velocity = forward * parentDaemon.WalkSpeed;
            yield return new WaitForEndOfFrame();
        }

        rbody.velocity = Vector3.zero;

        aa.Play(stopAnimation);
    }
}
