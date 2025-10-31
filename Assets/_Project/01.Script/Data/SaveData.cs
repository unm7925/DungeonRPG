using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // ← JsonUtility가 변환하려면 필수!
public class SaveData
{
    // 플레이어 데이터
    public int playerHP;
    public int playerMaxHP;
    public float playerPositionX;
    public float playerPositionY;
    
    // 인벤토리 데이터
    public List<string> inventoryItemNames = new List<string>();
    
    // 생성자 (저장할 때 사용)
    public SaveData(PlayerHealth health, PlayerInventory inventory)
    {
        // 체력 저장
        playerHP = health.GetCurrentHealth();
        playerMaxHP = health.GetMaxHealth();
        
        // 위치 저장
        Vector3 pos = health.transform.position;
        playerPositionX = pos.x;
        playerPositionY = pos.y;
        
        // 인벤토리 저장 (ItemData → 이름만)
        inventoryItemNames.Clear();
        foreach (ItemData item in inventory.inventory)
        {
            inventoryItemNames.Add(item.itemName);
        }
    }
}