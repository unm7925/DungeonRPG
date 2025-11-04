using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float lerpSpeed = 5f;
    
    private float targetHealth;
    private float displayHealth;

    void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
            
            targetHealth = playerHealth.GetCurrentHealth();
            displayHealth = playerHealth.GetCurrentHealth();

            UpdateColor();
        }
    }

    private void Update()
    {
        if (Mathf.Abs(displayHealth - targetHealth) > 0.01f)
        {
            displayHealth = Mathf.Lerp(displayHealth, targetHealth, lerpSpeed * Time.deltaTime);
            healthSlider.value = displayHealth;
            
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        if (fillImage == null) return;
        
        float healthPercentage = displayHealth / healthSlider.maxValue;

        if (healthPercentage > 0.5f)
        {
            fillImage.color = Color.green;
        }
        else if (healthPercentage > 0.25f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.red;
        }
        
    }

    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        // Debug.Log($"HealthBar Update: {currentHealth}/{maxHealth}");
        // 이벤트 구독이 잘 되어있는지 확인
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            targetHealth = currentHealth;
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}