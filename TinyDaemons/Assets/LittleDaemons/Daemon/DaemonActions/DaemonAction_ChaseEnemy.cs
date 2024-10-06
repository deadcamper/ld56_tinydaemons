using System.Collections;
using UnityEngine;

public class DaemonAction_ChaseEnemy : DaemonAction
{
    public float timeToChase = 1;
    public float distanceToStop = 1;

    public string walkAnimation = "Walk";

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        if (parentDaemon.enemy.Health <= 0)
        {
            parentDaemon.enemy = null;
        }
        if (parentDaemon.enemy == null)
        {
            Debug.LogError("DaemonChaseEnemy when Enemy is gone.");
            yield break;
        }

        float minWait = 0.25f;
        float timePassed = 0;

        UnityEngine.Animator aa = parentDaemon.body.animator;
        Rigidbody rbody = parentDaemon.GetComponent<Rigidbody>();
        aa.Play(walkAnimation);

        do
        {
            Vector3 enemyPos = parentDaemon.enemy.transform.position;
            Vector3 myPos = parentDaemon.transform.position;
            Vector3 dist = enemyPos - myPos;

            // Already close enough
            if (dist.sqrMagnitude < distanceToStop * distanceToStop)
            {
                aa.Play("Idle");
                rbody.velocity = Vector3.zero;
                yield return new WaitForSeconds(Mathf.Max(0f, timePassed - minWait));
                yield break;
            }

            Quaternion FromToRotation = Quaternion.FromToRotation(parentDaemon.body.transform.forward, dist);

            Vector3 euler = FromToRotation.eulerAngles;

            Quaternion lookDist = Quaternion.LookRotation(dist);
            Quaternion myLook = parentDaemon.body.transform.rotation;

            float degree = Quaternion.Angle(myLook, lookDist);
            float dot = Quaternion.Dot(myLook.normalized, lookDist.normalized);

            float degree2 = euler.y;

            // Rotate
            if (Mathf.Abs(degree2) > 5)
            {
                float signRotation = degree2 > 180 ? -1 : 1; //Mathf.Sign(degree);
                float rot = Time.deltaTime * parentDaemon.RotateSpeed * signRotation;
                parentDaemon.body.transform.Rotate(Vector3.up, rot);
            }

            // Move
            {
                Vector3 forward = parentDaemon.body.transform.TransformDirection(Vector3.forward);
                rbody.velocity = forward * parentDaemon.WalkSpeed;
            }

            yield return new WaitForEndOfFrame();
            timePassed += Time.deltaTime;
        }
        while (timePassed < timeToChase);

        Vector3 myEuler = parentDaemon.transform.rotation.eulerAngles;
    }
}
