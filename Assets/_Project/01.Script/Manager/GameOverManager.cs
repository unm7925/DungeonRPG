using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    
    // === UI 참조 ===
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject bossSpawn;
    [SerializeField] private PlayerController playerController;
    
    private BossAI currentBossAI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //현재는 단일 씬이라서
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // === Start - 초기화 ===
    private void Start()
    {
        // 패널 비활성화
        // playerController = GetComponent<PlayerController>();
        gameOverPanel.SetActive(false);
        AudioManager.Instance.PlayBGM("Bgm");
    }

    public void RegisterBoss(BossAI boss)
    {
        if (boss == null) return;
        currentBossAI = boss;
    }
    
    // === 게임 오버 표시 ===
    public void ShowGameOver()
    {
        // 1. 패널 활성화
        gameOverPanel.SetActive(true);
        AudioManager.Instance.PlaySFX("GameOver",1f);
        AudioManager.Instance.StopBgm();
        
        // 2. 게임 일시정지 (선택사항) 임시
        Time.timeScale = 0f;
    }
    
    
    // === Retry 버튼 (씬 재시작) ===
    public void OnRetryClicked()
    {
        // 1. 시간 복구
        Time.timeScale = 1f;
        
        // 2. Instance 초기화
        // Instance = null;
        
        // 3. 현재 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OnSavePointClicked()
    {
        
        Time.timeScale = 1f;
        
        SaveManager.Instance.LoadGame();
        AudioManager.Instance.PlayBGM("Bgm");
        gameOverPanel.SetActive(false);
        
        if(playerController != null)
        {
            playerController.enabled = true;
        }
        
        if(currentBossAI != null)
        {
            Destroy(currentBossAI.gameObject);
        }
        bossSpawn.SetActive(true);
    }
}