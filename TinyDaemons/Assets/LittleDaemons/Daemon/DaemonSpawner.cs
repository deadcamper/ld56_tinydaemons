using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DaemonSpawner : MonoBehaviour
{
    public Daemon playerPrefab;
    public List<Daemon> prefabDaemons;
    public List<Transform> spawnPoints;

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

            yield return new WaitForSeconds(timeToAct);

            Transform spawnPoint = spawnPoints[spawnPointIter];
            Vector3 position = spawnPoint.position;
            Quaternion rotation = spawnPoint.rotation;

            Daemon newDaemon = null;
            switch(decision)
            {
                case SpawnDecsion.DoNothing:
                    break;
                case SpawnDecsion.SpawnPlayer:
                    newDaemon = Instantiate(playerPrefab, position, playerPrefab.transform.rotation);
                    break;
                case SpawnDecsion.SpawnDaemon:
                    int daemonIndex = Random.Range(0, prefabDaemons.Count);
                    Daemon prefabDaemon = prefabDaemons[daemonIndex];
                    newDaemon = Instantiate(prefabDaemon, position, prefabDaemon.transform.rotation);
                    break;
            }

            // We rotate the body afterward
            if (newDaemon)
            {
                newDaemon.body.transform.rotation = rotation;
            }

            spawnPointIter = (1 + spawnPointIter) % spawnPoints.Count;
        }
    }
}
