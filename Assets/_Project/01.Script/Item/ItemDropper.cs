using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    // === 변수 ===
    [SerializeField] private ItemDropData dropData;
    
    // 드롭 위치 랜덤 범위
    [SerializeField] private float dropRadius = 1f;
    
    
    // === 아이템 드롭 시도 ===
    public void TryDropItems()
    {
        // dropData가 null이면 return
        if (dropData == null) return;
        
        // dropData.dropItems 배열 순회
        foreach (DropItem drop in dropData.dropItems)
        {
            // 랜덤 값 생성 (0.0 ~ 1.0)
            float randomValue = Random.Range(0f, 1f);
            
            // randomValue가 dropChance보다 작으면 드롭
            if (randomValue < drop.dropChance)
            {
                SpawnItem(drop);
            }
        }
    }
    
    
    // === 아이템 스폰 ===
    private void SpawnItem(DropItem drop)
    {
        // 1. 랜덤 오프셋 계산
        Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
        
        // 2. 스폰 위치 = 이 오브젝트 위치 + 랜덤 오프셋
        Vector3 spawnPos =transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
        
        // 3. 아이템 생성
        Instantiate(drop.itemPrefab, spawnPos, Quaternion.identity);
        
        // 4. 디버그 로그
        Debug.Log($"아이템 드롭: {drop.itemData.itemName}");
    }
}