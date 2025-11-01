
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waveDataList;

    private int currentWaveIndex = 0;

    private WaveData currentWave;
    
    private int spawnedEnemies = 0;
    
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

        spawnedEnemies = 0;
        aliveEnemies = 0;
        isWaveActive = true;
        
        Debug.Log($"Wave {currentWave.waveNumber}");
        
        waveUI?.ShowWaveStart(currentWave.waveNumber);

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (spawnedEnemies < currentWave.enemyCount)
        {
            SpawnEnemy();
            
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomY = Random.Range(-spawnRangeY, spawnRangeY);
        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);
        
        GameObject enemy = Instantiate(currentWave.enemyPrefab, spawnPos, Quaternion.identity);
        aliveEnemies++;
        spawnedEnemies++;
        
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

        if (aliveEnemies == 0 && spawnedEnemies == currentWave.enemyCount)
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
            GameVictory();
        }
    }

    IEnumerator RestAndNextWave()
    {
        yield return new WaitForSeconds(currentWave.restTime);
        
        StartWave();
    }

    private void GameVictory()
    {
        
        waveUI?.ShowVictory();
        Debug.Log("Game Victory");
    }
    
}