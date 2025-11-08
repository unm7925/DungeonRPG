
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("보스 문")]
    [SerializeField] private GameObject bossDoor;
    
    [SerializeField] private List<WaveData> waveDataList;

    private int currentWaveIndex = 0;

    private WaveData currentWave;
    
    private int totalSpawnedEnemies = 0;
    
    private int waveSpawnedEnemies = 0;
    
    private int aliveEnemies = 0;
    
    private bool isWaveActive = false;

    [SerializeField] private float spawnRangeX = 10f;
    [SerializeField] private float spawnRangeY = 10f;

    private WaveUIManager waveUI;

    private void Start()
    {
        waveUI = FindObjectOfType<WaveUIManager>();
        
        StartWave();
    }

    private void StartWave()
    {
        currentWave = waveDataList[currentWaveIndex];

        totalSpawnedEnemies = 0;
        aliveEnemies = 0;
        isWaveActive = true;
        
        Debug.Log($"Wave {currentWave.waveNumber}");
        
        waveUI?.ShowWaveStart(currentWave.waveNumber);

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        int totalEnemies = 0;

        List<int> remainingSpawns = new List<int>();
        
        for (int i = 0; i < currentWave.enemies.Count; i++)
        {
            totalEnemies += currentWave.enemies[i].count;
            remainingSpawns.Add(currentWave.enemies[i].count);
        }
        
        

        for (int i = 0; i < totalEnemies; i++)
        {
            int enemyType = GetRandomEnemyType(remainingSpawns);
            
            SpawnEnemy(enemyType);
            
            remainingSpawns[enemyType]--;
            
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
    }

    private int GetRandomEnemyType(List<int> remainingSpawns)
    {
        List<int> availableType = new List<int>();
        for (int i = 0; i < remainingSpawns.Count; i++)
        {
            if (remainingSpawns[i] > 0)
            {
                availableType.Add(i);
            }
        }
        
        return  availableType[Random.Range(0, availableType.Count)];
    }

    private void SpawnEnemy(int enemyPrefabsN)
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomY = Random.Range(-spawnRangeY, spawnRangeY);
        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);
        
        GameObject enemy = Instantiate(currentWave.enemies[enemyPrefabsN].enemyPrefab, spawnPos, Quaternion.identity); 
        aliveEnemies++;
        totalSpawnedEnemies++;
        waveSpawnedEnemies++;
        waveUI.UpdateEnemyCount(aliveEnemies);
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        
        if (enemyHealth != null)
        { 
            enemyHealth.OnDeath += OnEnemyDeath;
        }
        
    }

    private void OnEnemyDeath()
    {
        aliveEnemies--;

        Debug.Log(aliveEnemies);
        
        waveUI?.UpdateEnemyCount(aliveEnemies);
        int enemyAmount = 0;
        
        for (int i = 0; i < currentWave.enemies.Count; i++)
        {
            enemyAmount += currentWave.enemies[i].count;
        }
        
        if (aliveEnemies == 0 && totalSpawnedEnemies == enemyAmount)
        {
            WaveClear();
            
        }
        
    }

    private void WaveClear()
    {
        isWaveActive = false;
        
        Debug.Log($"Wave {currentWave.waveNumber} clear");

        waveUI?.ShowWaveClear();
        
        if (currentWaveIndex + 1 < waveDataList.Count)
        {
            currentWaveIndex++;

            StartCoroutine(RestAndNextWave());
        }
        else
        {
            bossDoor.SetActive(false);
        }
    }

    IEnumerator RestAndNextWave()
    {
        yield return new WaitForSeconds(currentWave.restTime);
        
        StartWave();
    }

}