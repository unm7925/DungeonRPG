using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("웨이브 정보")]
    public int waveNumber;           // 웨이브 번호 (1, 2, 3...)
    public int enemyCount;           // 이 웨이브에 스폰할 적 수
    public float spawnInterval;      // 적 스폰 간격 (초)
    public float restTime;           // 웨이브 클리어 후 휴식 시간
    
    [Header("적 프리팹")]
    public GameObject enemyPrefab;   // 스폰할 적 프리팹
}