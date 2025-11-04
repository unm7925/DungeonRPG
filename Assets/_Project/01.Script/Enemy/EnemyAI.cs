using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;  // 탐지 범위
    [SerializeField] private float attackRange = 1f;      // 공격 범위
    
    [Header("공격 설정")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    
    [Header("컴포넌트")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastMoveDirection =Vector2.down;
    
    private Transform player;
    private Rigidbody2D rb;
    private float attackTimer;
    
    // 상태 (Idle, Chase, Attack)
    private enum State { Idle, Chase, Attack }
    private State currentState = State.Idle;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Player 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure Player has 'Player' tag.");
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        UpdateAnimation();
        // 공격 쿨다운 감소
        attackTimer -= Time.deltaTime;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // 상태 전환 (State Machine)
        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chase;
                    Debug.Log($"{gameObject.name} detected player!");
                }
                break;
                
            case State.Chase:
                if (distanceToPlayer < attackRange)
                {
                    currentState = State.Attack;
                }
                else if (distanceToPlayer > detectionRange * 1.5f)
                {
                    currentState = State.Idle;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    ChasePlayer();
                }
                break;
                
            case State.Attack:
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Chase;
                }
                else
                {
                    AttackPlayer();
                }
                break;
        }
    }

    private void UpdateAnimation()
    {
        float speed = rb.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        if (speed > 0.1f)
        {
            Vector2 moveDirection = rb.velocity.normalized;
            animator.SetFloat("horizontal", moveDirection.x);
            animator.SetFloat("vertical", moveDirection.y);
            
            lastMoveDirection = moveDirection;
        }
        else
        {
            animator.SetFloat("horizontal", lastMoveDirection.x);
            animator.SetFloat("vertical", lastMoveDirection.y);
        }
    }

    void ChasePlayer()
    {
        // 플레이어 방향으로 이동
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }
    
    void AttackPlayer()
    {
        // 공격 중에는 멈추기
        rb.velocity = Vector2.zero;
        
        // 쿨다운 끝나면 공격
        if (attackTimer <= 0)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"{gameObject.name} attacked player!");
            }
            attackTimer = attackCooldown;
        }
    }
    
    // Gizmos로 범위 시각화 (Scene 뷰에서 보임)
    void OnDrawGizmosSelected()
    {
        // 탐지 범위 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // 공격 범위 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}