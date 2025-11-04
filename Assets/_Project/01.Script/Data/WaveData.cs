using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count;
}

[CreateAssetMenu(fileName = "New Wave", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("웨이브 정보")]
    public int waveNumber;           // 웨이브 번호 (1, 2, 3...)

    public float spawnInterval;      // 적 스폰 간격 (초)
    public float restTime;           // 웨이브 클리어 후 휴식 시간

    [Header("적 스폰 정보")]
    public List<EnemySpawnInfo> enemies; // Prefab + Count 묶음
    public bool randomSpawn = true;      // true면 섞어서, false면 순차
}