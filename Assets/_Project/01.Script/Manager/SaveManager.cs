using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // 1. 싱글톤 (나중에 설명)
    public static SaveManager Instance;
    
    // 2. 참조
    private PlayerHealth playerHealth;
    private PlayerInventory playerInventory;
    private InventoryUI inventoryUI;
    
    // 3. 저장 경로
    private string savePath;
    
    // 4. Awake - 싱글톤 초기화
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Instance가 null이면 this를 할당
        // Instance가 이미 있으면 this를 파괴
        // DontDestroyOnLoad(this) - 씬 전환해도 유지
    }
    
    // 5. Start - 참조 찾기
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        // playerHealth 찾기
        // playerInventory 찾기
        // inventoryUI 찾기
        savePath = Application.persistentDataPath + "/save.json";
        // savePath 설정
        // Application.persistentDataPath + "/save.json"
    }
    
    // 6. Update - F5,F9 키 감지
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadGame();
        }
    }
    
    // 7. 저장
    public void SaveGame()
    {
        SaveData data = new SaveData(playerHealth, playerInventory);
        string json = JsonUtility.ToJson(data, true);  // true = 보기 좋게
        File.WriteAllText(savePath, json);
        Debug.Log("save 성공");
        
        // SaveData 생성 (생성자로)
        
        // JsonUtility.ToJson()으로 JSON 변환
        
        // File.WriteAllText()로 파일 저장
        
        // Debug.Log("저장 완료!")
    }
    
    // 8. 불러오기
    public void LoadGame()
    {
        if (File.Exists(savePath) == false) return;
        // 파일 존재 확인 (File.Exists)
        
        // 파일 없으면 return
        
        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        
        // File.ReadAllText()로 JSON 읽기
        
        
        
        // JsonUtility.FromJson<SaveData>()로 객체 변환
        playerHealth.SetHealth(data.playerHP, data.playerMaxHP);

        // 플레이어 체력 복원
        Vector3 loadedPosition = new Vector3(data.playerPositionX, data.playerPositionY, 0f);
        playerHealth.transform.position = loadedPosition;
        // 플레이어 위치 복원
        Debug.Log("=== 인벤토리 불러오기 시작 ===");
        Debug.Log("저장된 아이템 개수: " + data.inventoryItemNames.Count);
        // 인벤토리 복원 (이름 → ItemData 찾기)

        // UI 업데이트
        playerInventory.inventory.Clear();

         // 2. 이름으로 ItemData 찾아서 추가
        foreach (string itemName in data.inventoryItemNames)
        {
            // Resources 폴더에서 찾기
            ItemData item = Resources.Load<ItemData>("Data/Items/" + itemName);
    
            if (item != null)
            {
                playerInventory.inventory.Add(item);
            }
        }
        inventoryUI.UpdateUI();
        Debug.Log("불러오기 완료!");
    }
}