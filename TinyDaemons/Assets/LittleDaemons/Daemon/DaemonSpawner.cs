using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DaemonSpawner : MonoBehaviour
{
    public Daemon playerPrefab;
    public List<Daemon> prefabDaemons;
    public List<SpawnPoint> spawnPoints;

    public float timeToCheck = 2.75f;
    public float timeToAct = 0.25f;

    public int maxDaemonCount = 6;

    private int activeDaemonCount;

    public enum SpawnDecsion
    {
        DoNothing,
        SpawnPlayer,
        SpawnDaemon
    }

    internal SpawnDecsion decision;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDownChecker());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CountDownChecker()
    {
        yield return new WaitForSeconds(timeToCheck + timeToAct); // Start one round no spawning
        int spawnPointIter = 0;
        while (true)
        {
            yield return new WaitForSeconds(timeToCheck);
            Daemon[] activeDaemons = GameObject.FindObjectsOfType<Daemon>();

            if (!activeDaemons.Any(dem => dem.daemonType == Daemon.DaemonType.Player))
            {
                decision = SpawnDecsion.SpawnPlayer;
            }
            else if (activeDaemons.Length < maxDaemonCount)
            {
                decision = SpawnDecsion.SpawnDaemon;
            }
            else
            {
                decision = SpawnDecsion.DoNothing;
            }

            Daemon chosenPrefab = null;

            switch (decision)
            {
                case SpawnDecsion.DoNothing:
                    chosenPrefab = null;
                    break;
                case SpawnDecsion.SpawnPlayer:
                    chosenPrefab = playerPrefab;
                    break;
                case SpawnDecsion.SpawnDaemon:
                    int daemonIndex = Random.Range(0, prefabDaemons.Count);
                    Daemon prefabDaemon = prefabDaemons[daemonIndex];
                    chosenPrefab = prefabDaemon;
                    break;
            }

            SpawnPoint spawnPoint = spawnPoints[spawnPointIter];
            if (chosenPrefab != null)
            {
                spawnPoint.PrimeToSpawn();
            }

            yield return new WaitForSeconds(timeToAct);

            spawnPoint.Spawn(chosenPrefab);

            spawnPointIter = (1 + spawnPointIter) % spawnPoints.Count;
        }
    }
}
