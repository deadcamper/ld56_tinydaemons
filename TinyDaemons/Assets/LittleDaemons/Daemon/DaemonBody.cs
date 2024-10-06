using System.Collections.Generic;
using UnityEngine;

public class DaemonBody : MonoBehaviour
{
    public Animator animator;

    public void ExplodeToGibs()
    {
        Collider[] partColliders = GetComponentsInChildren<Collider>(true);

        Transform parentTransform = GetComponentInParent<Daemon>().transform.parent;

        HashSet<GameObject> gibs = new HashSet<GameObject>();

        foreach(Collider col in partColliders)
        {
            gibs.Add(col.gameObject);
            col.enabled = true;
        }

        foreach (GameObject gib in gibs)
        {
            Rigidbody rb = gib.AddComponent<Rigidbody>();
            Giblet g = gib.AddComponent<Giblet>();
            Vector3 random = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.AddForce(random, ForceMode.Impulse);
            gib.transform.parent = parentTransform;
        }

        Destroy(gameObject);
    }
}
