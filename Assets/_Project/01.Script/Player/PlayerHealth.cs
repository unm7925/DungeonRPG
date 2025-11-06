using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("체력 설정")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    [Header("무적 설정")]
    [SerializeField] private float invincibleDuration = 1f;
    private float invincibleTimer;
    private bool isInvincible;

    void Start()
    {
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
                
                InvincibilityEffect invEffect = GetComponent<InvincibilityEffect>();
                if (invEffect != null)
                {
                    Debug.Log("StopBlinking 호출!");  // ← 추가
                    invEffect.StopBlinking();
                }
                else
                {
                    Debug.LogWarning("InvincibilityEffect 못 찾음!");  // ← 추가
                }
            }
        }
        /*
        플레이어 체력변화로 인한 체력바 변환을 확인하기 위함
        if (Input.GetKeyDown(KeyCode.Alpha1))  // 키보드 1번
        {
            TakeDamage(10);
            Debug.Log("Test: Take 10 damage");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))  // 키보드 2번
        {
            Heal(20);
            Debug.Log("Test: Heal 20");
        }
        */
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        HitEffect hitEffect = GetComponent<HitEffect>();
        if (hitEffect != null)
        {
            hitEffect.PlayHitEffect();
        }

        if (currentHealth <= 0)
        {
            OnHealthChanged?.Invoke(0, maxHealth);
            Die();
        }
        else
        {
           
            
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            isInvincible = true;
            invincibleTimer = invincibleDuration;
            
            InvincibilityEffect invEffect = GetComponent<InvincibilityEffect>();
            if (invEffect != null)
            {
                invEffect.StartBlinking();
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        OnDeath?.Invoke();
        AudioManager.Instance.PlaySFX("videoplayback");
        Debug.Log("Player Died!");
        
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            //임시
            PlayerController playerController = FindObjectOfType<PlayerController>();
            playerController.enabled = false;
            gameOverManager.ShowGameOver();
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    
    public void SetHealth(int hp, int maxHp)
    {
        maxHealth = maxHp;
        currentHealth = hp;
    
        // 체력바 UI 업데이트 (이벤트 있으면)
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}