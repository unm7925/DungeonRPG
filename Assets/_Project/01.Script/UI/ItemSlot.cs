
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // 1. 아이콘 Image 참조
    [SerializeField] private Image iconImage;
    
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryUI inventoryUI;
    
    // 2. 현재 슬롯에 있는 아이템
    private ItemData currentItem;
    [SerializeField] private int slotIndex; 
    
    // 3. 슬롯에 아이템 설정
    public void SetItem(ItemData item)
    {
        currentItem = item;
        // currentItem에 저장
        
        // item이 null이 아니면
        if (item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
            // 아이콘 이미지의 sprite를 item.icon으로 설정
            // 아이콘 활성화 (iconImage.enabled = true)
        }
        else // item이 null이면 (빈 슬롯)
        {
            iconImage.enabled = false;
            // 아이콘 비활성화 (iconImage.enabled = false)
        }
    }
    
    // 4. (선택) 슬롯 비우기
    public void ClearSlot()
    {
        SetItem(null);
    }
    public void OnSlotClicked()
    {
        // 1. 빈 슬롯이면 무시
        if (currentItem == null)
        {
            Debug.Log("빈 슬롯!");
            return;
        }
        
        AudioManager.Instance.PlaySFX("useitem",1f);
        
        // 2. 아이템 타입 확인
        if (currentItem.itemType == ItemType.HealthPotion)
        {
            // 3. 플레이어 찾기
            
            
                // 4. 체력 회복
                playerHealth.Heal(currentItem.effectValue);
                
                // 5. 인벤토리에서 제거
                playerInventory.RemoveItemAt(slotIndex);
                
                // 6. UI 업데이트
                if (inventoryUI != null)
                {
                    inventoryUI.UpdateUI();
                }
            
        }
        else if (currentItem.itemType == ItemType.PowerUpPotion)
        {
            
            
                // 4. 공격력 증가
            if(playerAttack != null)
            {
                playerAttack.UpgradeAttack(currentItem.effectValue);
            }
            // 5. 인벤토리에서 제거
            if(playerInventory != null)
            {
                playerInventory.RemoveItemAt(slotIndex);
            }
            // 6. UI 업데이트
            
            inventoryUI.UpdateUI();
            
            
        }
    }
}