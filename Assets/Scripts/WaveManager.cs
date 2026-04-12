using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class ZombieSpawnInfo
    {
        public GameObject zombiePrefab;
        public int count;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<ZombieSpawnInfo> zombiesToSpawn;
        public float timeBetweenSpawns = 0.5f;
        public float timeBeforeWave = 3f;
    }

    [Header("Wave Settings")]
    public List<Wave> waves;
    public List<Transform> spawnPoints;

    [Header("State")]
    public int currentWaveIndex = 0;
    public bool autoStartNextWave = true;
    public float timeBetweenWaves = 5f;

    private int totalZombiesInWave;
    private int zombiesKilled;
    private bool waveInProgress;
    private List<GameObject> activeZombies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(StartWave(currentWaveIndex));
    }

    IEnumerator StartWave(int waveIndex)
    {
        if (waveIndex >= waves.Count)
        {
            Debug.Log("All waves completed!");
            yield break;
        }

        Wave wave = waves[waveIndex];
        waveInProgress = true;
        zombiesKilled = 0;
        totalZombiesInWave = 0;
        activeZombies.Clear();

        foreach (var info in wave.zombiesToSpawn)
            totalZombiesInWave += info.count;

        Debug.Log($"Wave {waveIndex + 1} starting: {wave.waveName}");
        yield return new WaitForSeconds(wave.timeBeforeWave);

        foreach (var spawnInfo in wave.zombiesToSpawn)
        {
            for (int i = 0; i < spawnInfo.count; i++)
            {
                SpawnZombie(spawnInfo.zombiePrefab);
                yield return new WaitForSeconds(wave.timeBetweenSpawns);
            }
        }
    }

    void SpawnZombie(GameObject prefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject zombie = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        activeZombies.Add(zombie);

        // Hook into death
        EnemyHealth health = zombie.GetComponent<EnemyHealth>();
        if (health != null)
            health.onDeath += OnZombieKilled;
    }

    void OnZombieKilled()
    {
        zombiesKilled++;
        Debug.Log($"Zombies killed: {zombiesKilled}/{totalZombiesInWave}");

        if (zombiesKilled >= totalZombiesInWave)
        {
            waveInProgress = false;
            Debug.Log($"Wave {currentWaveIndex + 1} complete!");

            if (autoStartNextWave)
                StartCoroutine(NextWave());
        }
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        currentWaveIndex++;
        StartCoroutine(StartWave(currentWaveIndex));
    }
}