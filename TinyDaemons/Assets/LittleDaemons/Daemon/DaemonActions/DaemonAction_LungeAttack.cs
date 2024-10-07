using System.Collections;
using UnityEngine;

public class DaemonAction_LungeAttack : DaemonAction
{
    public string attackAnimation = "AttackLunge";

    public float speedMultiplier = 3f;

    public float distToLunge = 3f;

    public float radiusToDoDamage = 5f;

    public int LungeDamage = 25;

    public bool LungeAwayFromDanger = false;

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        UnityEngine.Animator aa = parentDaemon.body.animator;
        yield return new WaitForEndOfFrame();

        Vector3 lungeDir;
        if (parentDaemon.enemy)
        {
            lungeDir = parentDaemon.enemy.transform.position - parentDaemon.transform.position;
            if (LungeAwayFromDanger)
            {
                lungeDir *= -1;
            }
        }
        else
        {
            lungeDir = parentDaemon.body.transform.forward;
        }

        parentDaemon.body.transform.forward = lungeDir;
        lungeDir = lungeDir.normalized;

        aa.Play(attackAnimation);
        Rigidbody rbody = parentDaemon.GetComponent<Rigidbody>();

        

        float timeToLunge = distToLunge / (speedMultiplier * parentDaemon.WalkSpeed);

        float time = 0;
        while (time < timeToLunge)
        {
            time += Time.deltaTime;

            rbody.velocity = lungeDir * parentDaemon.WalkSpeed * speedMultiplier;
            yield return new WaitForEndOfFrame();
        }

        if (parentDaemon.enemy)
        {
            lungeDir = parentDaemon.enemy.transform.position - parentDaemon.transform.position;

            if (lungeDir.sqrMagnitude < radiusToDoDamage*radiusToDoDamage)
            {
                parentDaemon.enemy.Hurt(LungeDamage);
            }
        }

        rbody.velocity = Vector3.zero;

        aa.Play("Idle");
    }
}
