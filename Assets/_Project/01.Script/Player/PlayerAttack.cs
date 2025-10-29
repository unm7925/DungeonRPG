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
            // EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            // if (enemyHealth != null)
            // {
            //     enemyHealth.TakeDamage(attackDamage);
            // }
            Debug.Log($"Attack! Hit range check");
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