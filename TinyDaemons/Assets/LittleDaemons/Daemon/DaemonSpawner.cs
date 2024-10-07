using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DaemonSpawner : MonoBehaviour
{
    // One of each is required
    public List<Daemon> essentialDaemons = new List<Daemon>();
    public Daemon playerPrefab;

    // Side daemons
    [FormerlySerializedAs("prefabDaemons")]
    public List<Daemon> sideDaemons;
    public List<SpawnPoint> spawnPoints;

    public float timeToStart = 0f;
    public float timeToCheck = 2.75f;
    public float timeToAct = 0.25f;

    [FormerlySerializedAs("maxDaemonCount")]
    public int maxSideDaemonCount = 6;

    private int activeDaemonCount;

    /*
    public enum SpawnDecsion
    {
        DoNothing,
        SpawnPlayer,
        SpawnDaemon
    }

    internal SpawnDecsion decision;
    */

    // Start is called before the first frame update
    void Start()
    {
        if (playerPrefab)
        {
            essentialDaemons.Add(playerPrefab);
        }
        StartCoroutine(CountDownChecker());
    }

    private IEnumerator CountDownChecker()
    {
        if (timeToStart > 0f)
        {
            yield return new WaitForSeconds(timeToStart); // Added delay
        }
        
        int spawnPointIter = 0;
        while (true)
        {
            yield return new WaitForSeconds(timeToCheck);

            Daemon chosenPrefab = EvalNextPrefabToSpawn();
            SpawnPoint spawnPoint = null;

            if (chosenPrefab != null)
            {
                spawnPoint = spawnPoints[spawnPointIter];
                spawnPoint.PrimeToSpawn();
                spawnPointIter = (1 + spawnPointIter) % spawnPoints.Count;

                yield return new WaitForSeconds(timeToAct);

                spawnPoint.Spawn(chosenPrefab);
            }
        }
    }

    private Daemon EvalNextPrefabToSpawn()
    {
        Daemon chosenPrefab = null;

        HashSet<Daemon> activeDaemons = FindObjectsOfType<Daemon>().ToHashSet();

        // Evaluate essential daemons
        foreach (Daemon essDaemon in essentialDaemons)
        {
            Daemon.DaemonType essentialType = essDaemon.daemonType;
            if (essentialType == Daemon.DaemonType.None)
            {
                Debug.LogWarning("Treating 'None' type as essential. May want to swap this asap...", essDaemon);
            }

            Daemon liveDaemon = activeDaemons.Where(act => act.daemonType == essentialType).FirstOrDefault();
            if (liveDaemon)
            {
                activeDaemons.Remove(liveDaemon);
                continue;
            }
            else
            {
                // This prefab is missing and we need a new one pronto!
                chosenPrefab = essDaemon;
                break;
            }
        }

        // Keep looking?
        if (chosenPrefab == null && sideDaemons.Count > 0)
        {
            // Determine if we need more Daemons after essentials.
            if (activeDaemons.Count >= maxSideDaemonCount)
            {
                chosenPrefab = null;
            }
            else
            {
                int rngIndex = Random.Range(0, sideDaemons.Count);
                Daemon prefabDaemon = sideDaemons[rngIndex];
                chosenPrefab = prefabDaemon;
            }
        }

        return chosenPrefab;
    }
}
