using System.Data;
using System.IO;
using _Project._01.Script.UI;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // 1. 싱글톤 (나중에 설명)
    public static SaveManager Instance;
    
    // 2. 참조
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private PlayerAttack playerAttack;
    
    public WaveManager waveManager;
    // 3. 저장 경로
    private string savePath;
    
    // 4. Awake - 싱글톤 초기화
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 단일 씬 게임
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Instance가 null이면 this를 할당
        // Instance가 이미 있으면 this를 파괴
        // DontDestroyOnLoad(this) - 씬 전환해도 유지
    }
    
    // Start - 참조 찾기
    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.json";
        // savePath 설정
        // Application.persistentDataPath + "/save.json"
    }
    
    
    private void Update()
    {
        /* 저장기능 테스트
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //SaveGame();
            //SaveGameDev();
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadGame();
        }
        */
    }
    
    // 7. 저장
    public void SaveGame(SaveData data)
    {
        
        string json = JsonUtility.ToJson(data, true);  // true = 보기 좋게
        File.WriteAllText(savePath, json);
        Debug.Log("save 성공");
        
        // SaveData 생성 (생성자로)
        
        // JsonUtility.ToJson()으로 JSON 변환
        
        // File.WriteAllText()로 파일 저장
        
        // Debug.Log("저장 완료!")
    }
    
    /* 저장기능 테스트 용 
    public void SaveGameDev()
    {
        SaveData data = new SaveData(playerHealth, playerInventory,playerAttack,true);
        string json = JsonUtility.ToJson(data, true);  // true = 보기 좋게
        File.WriteAllText(savePath, json);
        Debug.Log("save 성공");
        
        // SaveData 생성 (생성자로)
        
        // JsonUtility.ToJson()으로 JSON 변환
        
        // File.WriteAllText()로 파일 저장
        
        // Debug.Log("저장 완료!")
    }
    */
    
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
        
        Vector2 loadedPosition = data.playerPosition;
        playerHealth.transform.position = loadedPosition;
        // 플레이어 위치 복원
        
        playerAttack.SetAttackDamage(data.playerDamage);
        // 플레이어 공격력 복원
        
        
        // UI 업데이트
        playerInventory.inventory.Clear();

        waveManager.SetWaveCleared(data.isWaveCleared);
        
         // 이름으로 ItemData 찾아서 추가
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