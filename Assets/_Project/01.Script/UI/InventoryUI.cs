using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // 1. 참조
    [SerializeField] private GameObject inventoryPanel;  // 인벤토리 패널
    [SerializeField] private Transform slotContainer;    // 슬롯들의 부모
    
    private List<ItemSlot> slots = new List<ItemSlot>(); // 슬롯 리스트
    private PlayerInventory playerInventory;             // 플레이어 인벤토리
    
    // 2. 초기화
    private void Start()
    {
        // PlayerInventory 찾기
        playerInventory = FindObjectOfType<PlayerInventory>(); // 찾기

        foreach (Transform child in slotContainer)
        {
            ItemSlot slot = child.GetComponent<ItemSlot>();
            if (slot != null)
            {
                slots.Add(slot);
            }
        }
        // slotContainer의 자식들을 모두 가져와서 slots 리스트에 추가
        // foreach로 slotContainer의 모든 자식 순회
        // GetComponent<ItemSlot>()으로 ItemSlot 가져오기
        // slots.Add()로 리스트에 추가
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged += UpdateUI;
        }
        
        // 인벤토리 패널 비활성화
        inventoryPanel.SetActive(false);
    }

    
    
    // 3. Update에서 I 키 감지
    private void Update()
    {
        // I 키를 누르면
        if (Input.GetKeyDown(KeyCode.I))
        {
            // 인벤토리 토글 (열기/닫기)
            ToggleInventory();
        }
    }
    
    // 4. 인벤토리 열기/닫기
    private void ToggleInventory()
    {
        // 현재 활성화 상태의 반대로 설정
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive); //inventoryPanel의 현재 활성화 상태;
        
        
        // 열렸으면 UI 업데이트
        if (!isActive)  // 열렸을 때
        {
            UpdateUI();
        }
    }
    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= UpdateUI;
        }
    }
    // 5. UI 업데이트
    public void UpdateUI()
    {
        // playerInventory의 아이템 리스트 가져오기
        // (일단 public으로 접근하거나, Get 메서드 필요)
        
        // 모든 슬롯 순회
        for (int i = 0; i < slots.Count; i++)
        {
            
            // i번째 아이템이 있으면
            if ( i < playerInventory.inventory.Count)
            {

                slots[i].SetItem(playerInventory.inventory[i]); //(인벤토리의 i번째 아이템)
            }
            else  // 아이템 없으면
            {
                slots[i].ClearSlot();
            }
        }
    }
}