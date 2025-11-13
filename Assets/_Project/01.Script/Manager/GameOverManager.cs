using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // === UI 참조 ===
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject bossSpawn;
    
    // === Start - 초기화 ===
    private void Start()
    {
        // 패널 비활성화
        gameOverPanel.SetActive(false);
        AudioManager.Instance.PlayBGM("Bgm");
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
        
        // 2. 현재 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OnSavePointClicked()
    {
        Time.timeScale = 1f;
        
        SaveManager.Instance.LoadGame();
        AudioManager.Instance.PlayBGM("Bgm");
        gameOverPanel.SetActive(false);
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.enabled = true;
        
        BossAI bossAI = FindObjectOfType<BossAI>();
        Destroy(bossAI.gameObject);
        bossSpawn.SetActive(true);
    }
}