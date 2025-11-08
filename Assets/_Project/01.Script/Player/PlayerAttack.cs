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
    
    [Header("데미지 텍스트")]
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Canvas uiCanvas; // ← Screen Space Overlay 캔버스
    
    [Header("공격 이펙트")]
    [SerializeField] private GameObject effectPrefab;
    
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

        // 캔버스 없으면 자동으로 찾기 (한 번만 실행)
        if (uiCanvas == null)
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (Canvas canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    uiCanvas = canvas;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (InventoryUI.Instance.isOpen)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && attackTimer <= 0)
        {
            Attack();
            AudioManager.Instance.PlaySFX("Attack(Sword)",1.5f);
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
        
        if(effectPrefab != null)
        {
            float angle = Mathf.Atan2(attackDirection.y,attackDirection.x) * Mathf.Rad2Deg;

            angle += 180f;
            Quaternion rotation = Quaternion.Euler(0,0,angle);
            GameObject effect = Instantiate(effectPrefab, attackPoint.position, rotation);
            Destroy(effect, 0.5f);
        }
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
    void ShowDamageText(Vector3 worldPosition, int damage)
    {
        if (damageTextPrefab == null || uiCanvas == null) return;

        // 데미지 텍스트 생성
        GameObject damageTextObj = Instantiate(damageTextPrefab);
        RectTransform rectTransform = damageTextObj.GetComponent<RectTransform>();

        // Canvas의 자식으로 설정
        rectTransform.SetParent(uiCanvas.transform, false);

        // DamageText 초기화
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        if (damageText != null)
        {
            damageText.Initialize(worldPosition, uiCanvas);
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