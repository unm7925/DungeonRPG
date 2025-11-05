using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // 1. 인벤토리 (아이템 리스트)
    public List<ItemData> inventory = new List<ItemData>();
    
    public delegate void InventoryChangedDelegate();
    
    public event InventoryChangedDelegate OnInventoryChanged;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))  // I 키 누르면
        {
            PrintInventory();  // 인벤토리 출력
        }
    }
    // 2. 아이템 추가 메서드
    public void AddItem(ItemData item)
    {
        // 인벤토리에 아이템 추가
        inventory.Add(item);
        // 디버그 로그 출력
        Debug.Log("아이템 획득: " + item.itemName);
        
        OnInventoryChanged?.Invoke();
    }
    
    // 3. (선택) 인벤토리 확인 메서드
    public void PrintInventory()
    {
        Debug.Log("=== 인벤토리 ===");

        foreach (ItemData item in inventory)
        {
            Debug.Log(item.itemName);
        }
        // foreach로 inventory를 돌면서
        // 각 아이템의 이름 출력
    }
    // 아이템 제거
 
// 또는 인덱스로 제거
    public void RemoveItemAt(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            ItemData item = inventory[index];
            inventory.RemoveAt(index);
            Debug.Log($"아이템 제거: {item.itemName}");
            
            OnInventoryChanged?.Invoke();
        }
    }
}