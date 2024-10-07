using UnityEngine;
using static DaemonSpawner;

public class SpawnPoint : MonoBehaviour
{
    public Animation spawnEffect;

    public Transform spawnPointLocation;

    // Start is called before the first frame update
    void Start()
    {
        spawnEffect.gameObject.SetActive(false);
    }

    public void PrimeToSpawn()
    {
        spawnEffect.gameObject.SetActive(true);
    }

    public void Spawn(Daemon prefab)
    {
        Vector3 position = spawnPointLocation.position;
        Quaternion rotation = spawnPointLocation.rotation;

        Daemon newDaemon = Instantiate(prefab, position, prefab.transform.rotation);

        // We rotate the body afterward
        if (newDaemon)
        {
            newDaemon.body.transform.rotation = rotation;
        }

        spawnEffect.gameObject.SetActive(false);
    }
}
