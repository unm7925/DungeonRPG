using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 0.5f;

    [Header("공격 판정")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    
    [Header("데미지 텍스트")] // ← 추가!
    [SerializeField] private GameObject damageTextPrefab;

    private PlayerController playerController;
    private Animator animator;
    private float attackTimer;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        if (attackPoint == null)
        {
            GameObject point = new GameObject("AttackPoint");
            point.transform.SetParent(transform);
            point.transform.localPosition = Vector3.zero;
            attackPoint = point.transform;
        }
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && attackTimer <= 0)
        {
            Attack();
            AudioManager.Instance.PlaySFX("videoplayback");
        }
    }

    void Attack()
    {
        attackTimer = attackCooldown;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Vector2 attackDirection = playerController.GetLastMoveDirection();
        attackPoint.localPosition = attackDirection * 0.5f;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (var enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                ShowDamageText(enemy.transform.position, attackDamage);
            }
        }
    }
    void ShowDamageText(Vector3 position, int damage)
    {
        if (damageTextPrefab == null) return;
        
        // 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        
        // 데미지 텍스트 생성
        GameObject damageTextObj = Instantiate(damageTextPrefab, screenPos, Quaternion.identity);
        
        // Canvas의 자식으로 설정
        Canvas canvas = FindObjectOfType<Canvas>();
        damageTextObj.transform.SetParent(canvas.transform, false);
        damageTextObj.transform.position = screenPos;
        
        // 데미지 값 설정
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        if (damageText != null)
        {
            damageText.SetDamage(damage);
        }
    }
    public void UpgradeAttack(int amount)
    {
        attackDamage += amount;
        Debug.Log($"Attack upgraded! New damage: {attackDamage}");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}