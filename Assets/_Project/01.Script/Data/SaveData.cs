using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // ← JsonUtility가 변환하려면 필수!
public class SaveData
{
    // 플레이어 데이터
    public int playerHP;
    public int playerMaxHP;
    public int playerDamage;
    public Vector2 playerPosition;
    
    public bool isWaveCleared;  // Wave 클리어 여부
    public Vector2 checkpointPosition;  // 체크포인트 위치
    
    // 인벤토리 데이터
    public List<string> inventoryItemNames = new List<string>();
    
    // 생성자 (저장할 때 사용)
    public SaveData(PlayerHealth health, PlayerInventory inventory, PlayerAttack attack, bool waveCleared)
    {
        // 체력 저장
        playerHP = health.GetCurrentHealth();
        playerMaxHP = health.GetMaxHealth();
        
        // 공격력 저장
        playerDamage = attack.GetPlayerDamage();

        // 위치 저장
        Vector2 pos = health.transform.position;
        playerPosition = pos;

        // 인벤토리 저장 (ItemData → 이름만)
        inventoryItemNames.Clear();
        foreach (ItemData item in inventory.inventory)
        {
            inventoryItemNames.Add(item.itemName);
        }
        
        // 웨이브 클리어 저장
        isWaveCleared = waveCleared;
    }
}