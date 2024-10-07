using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DaemonAction_PlayAnimation : DaemonAction
{
    [FormerlySerializedAs("animationName")]
    public string animationState;
    public bool waitForCompletion;

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        UnityEngine.Animator aa = parentDaemon.body.animator;

        aa.StopPlayback();
        aa.Play(animationState);
        //aa.
        yield return new WaitForEndOfFrame();
        if (waitForCompletion)
        {
            while (waitForCompletion)
            {
                var curState = aa.GetCurrentAnimatorStateInfo(0);

                if (!curState.IsName(animationState))
                {
                    break;
                }
                if (1 <= curState.normalizedTime)
                {
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
            aa.StopPlayback();
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
        }
    }

}
