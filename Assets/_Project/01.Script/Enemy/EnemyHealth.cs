using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("체력 설정")]
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;
    
    // 이벤트 (적이 죽을 때 알림)
    public event System.Action OnDeath;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}/{maxHealth}");
        HitEffect hitEffect = GetComponent<HitEffect>();
        if (hitEffect != null)
        {
            hitEffect.PlayHitEffect();
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        
        OnDeath?.Invoke();  // 이벤트 발동!
        ItemDropper dropper = GetComponent<ItemDropper>();
        if (dropper != null)
        {
            dropper.TryDropItems();
        }
        Destroy(gameObject);
    }
    
    // 현재 체력 확인용 (디버그)
    public int GetCurrentHealth() => currentHealth;
}