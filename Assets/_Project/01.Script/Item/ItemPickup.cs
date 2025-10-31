using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // 1. 이 아이템의 데이터
    [SerializeField] private ItemData itemData;
    
    // 2. 충돌 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 3. Player 태그 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 4. PlayerInventory 가져오기
            PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();
            
            // 5. null 체크 (안전장치)
            if (inventory != null )
            {
                // 6. 아이템 추가
                inventory.AddItem(itemData);
                
                // 7. 이 오브젝트 삭제
                Destroy(this.gameObject);
            }
        }
    }
}