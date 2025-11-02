using UnityEngine;

[System.Serializable]
public class DropItem
{
    public ItemData itemData;      // 드롭할 아이템
    public float dropChance;       // 드롭 확률 (0.0 ~ 1.0)
    public GameObject itemPrefab;  // 스폰할 프리팹
}

[CreateAssetMenu(fileName = "New Drop Table", menuName = "Game/Item Drop Data")]
public class ItemDropData : ScriptableObject
{
    public DropItem[] dropItems;   // 드롭 가능한 아이템 리스트
}