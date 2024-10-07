using System.Collections;
using UnityEngine;

public class DaemonProjectile : MonoBehaviour
{
    public float speed;

    public int damage;

    public int randomizeMultiplier; // 0 and 1 do nothing

    internal Daemon source;
    internal Vector3 direction; // normalized direction

    public void Launch(Daemon source, Vector3 direction)
    {
        this.source = source;
        this.direction = direction.normalized;

        // Hopefully it was spawned right!
        StartCoroutine(Travel());
    }

    private IEnumerator Travel()
    {
        float timeToDie = 30;
        while (timeToDie > 0)
        {
            yield return new WaitForEndOfFrame();
            timeToDie -= Time.deltaTime;

            gameObject.transform.position = gameObject.transform.position + (direction * Time.deltaTime * speed);
        }
        Destruct();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destruct();
        }
        if (other.gameObject.CompareTag("Daemon"))
        {
            Daemon daemon = other.GetComponent<Daemon>();
            if (daemon != source)
            {
                int hitpoints = EvalDamage();

                daemon.Hurt(hitpoints);

                Destruct();
            }
        }
    }

    private int EvalDamage()
    {
        int multiple = 1;
        if (randomizeMultiplier > 1)
        {
            multiple = Random.Range(1, randomizeMultiplier);
        }

        return damage * multiple;
    }

    private void Destruct()
    {
        // TODO - Other behaviors?


        Destroy(gameObject);
    }
}
