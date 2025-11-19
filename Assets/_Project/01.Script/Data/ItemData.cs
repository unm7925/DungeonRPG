using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;         // 아이템 이름
    public Sprite icon;             // 아이템 아이콘
    
    [Header("효과")]
    public ItemType itemType;       // 아이템 종류
    public int effectValue;         // 효과 수치 (체력 회복량 등)
    
    [TextArea(3, 5)]
    public string description;      // 아이템 설명
}

public enum ItemType
{
    HealthPotion, // 체력 회복
    PowerUpPotion // 공격력 강화
}