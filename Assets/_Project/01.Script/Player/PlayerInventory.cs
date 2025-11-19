using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // 인벤토리 (아이템 리스트)
    public List<ItemData> inventory = new List<ItemData>();
    
    public delegate void InventoryChangedDelegate();
    
    public event InventoryChangedDelegate OnInventoryChanged;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))  // I 키 누르면
        {
            //PrintInventory();  // 인벤토리 출력
        }
    }
    // 아이템 추가 메서드
    public void AddItem(ItemData item)
    {
        // 인벤토리에 아이템 추가
        inventory.Add(item);
        
        OnInventoryChanged?.Invoke();
    }
    
    // 인벤토리 확인 메서드
    /*
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
    */
    
    // 아이템 인덱스 제거 메서드
    public void RemoveItemAt(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            ItemData item = inventory[index];
            inventory.RemoveAt(index);
            
            
            OnInventoryChanged?.Invoke();
        }
    }
}