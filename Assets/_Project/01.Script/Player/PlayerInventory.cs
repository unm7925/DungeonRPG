using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // 1. 인벤토리 (아이템 리스트)
    public List<ItemData> inventory = new List<ItemData>();
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
}