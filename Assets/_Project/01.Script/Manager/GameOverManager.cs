using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // === UI 참조 ===
    [SerializeField] private GameObject gameOverPanel;
    
    
    // === Start - 초기화 ===
    private void Start()
    {
        // 패널 비활성화
        gameOverPanel.SetActive(false);
    }
    
    
    // === 게임 오버 표시 ===
    public void ShowGameOver()
    {
        // 1. 패널 활성화
        gameOverPanel.SetActive(true);
        
        // 2. 게임 일시정지 (선택사항)
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
}