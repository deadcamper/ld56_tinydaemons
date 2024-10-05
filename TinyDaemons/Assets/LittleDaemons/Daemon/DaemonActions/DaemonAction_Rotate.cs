using System.Collections;
using UnityEngine;

public class DaemonAction_Rotate : DaemonAction
{
    public bool random;

    public string rotationAnimation = "Idle";

    public float rotation = 90f;
    public float rotationMinimum = 0f;

    protected override IEnumerator InternalAction(Daemon parentDaemon)
    {
        UnityEngine.Animator aa = parentDaemon.body.animator;

        aa.Play(rotationAnimation);

        Rigidbody rbody = parentDaemon.GetComponent<Rigidbody>();
        rbody.velocity = Vector3.zero;

        float useRotation = rotation;
        if (random)
        {
            useRotation = Random.Range(rotationMinimum, rotation);
            useRotation *= Mathf.Sign(Random.Range(-100f, 100f));
        }

        float signRotation = Mathf.Sign(useRotation);

        float timeToSpeen = Mathf.Abs(useRotation) / parentDaemon.RotateSpeed;
        float time = 0;
        while (time < timeToSpeen)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            float rot = Time.deltaTime * parentDaemon.RotateSpeed * signRotation;
            parentDaemon.body.transform.Rotate(Vector3.up,rot);
        }

        rbody.velocity = Vector3.zero;
    }
}
