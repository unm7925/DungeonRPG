
using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed =1.5f;
    [SerializeField] private float detectionRange = 7f;
    [SerializeField] private float preferredDistance = 4f;
    
    [Header("공격 설정")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float attackCooldown = 2f;
    
    [Header("컴포넌트")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastMoveDirection = Vector2.down;

    private Transform player;
    private Rigidbody2D rb;
    private float attackTimer;

    private enum State
    {
        Idle,
        KeepDistance,
        Attack
    }
    private State currentState = State.Idle;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        attackTimer = attackCooldown;
        GameObject  playerObj = GameObject.FindWithTag("Player");
        if(playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        UpdateAnimation();
        
        attackTimer -= Time.deltaTime;
        
        float  distanceToPlayer = Vector2.Distance(transform.position,player.position );

        switch (currentState)
        {
            case State.Idle:
                rb.velocity = Vector2.zero;
                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.KeepDistance;
                }
                break;
            case State.KeepDistance:
                
                if (Mathf.Abs(distanceToPlayer - preferredDistance) < 0.5f)
                {
                    currentState = State.Attack;
                }
                else if ( distanceToPlayer > detectionRange * 1.5f)
                {
                    currentState = State.Idle;
                }
                else
                {
                    KeepDistance(distanceToPlayer);
                }
                break;
            case State.Attack:
                rb.velocity = Vector2.zero;
                if (distanceToPlayer > preferredDistance +1f || distanceToPlayer <= preferredDistance -1f)
                {
                    currentState = State.KeepDistance;
                }
                else if(attackTimer <= 0)
                {
                    AudioManager.Instance.PlaySFX("Attack(bow)",0.7f);
                    ShootProjectile();
                    
                }
                

                break;
        }
    }

    private void KeepDistance(float currentDistance)
    {
        
        Vector2 direction = (player.position - transform.position).normalized;

        if (currentDistance > preferredDistance+ 0.5f)
        {
            rb.velocity = direction*moveSpeed;
        }
        else if (currentDistance < preferredDistance-1f)
        {
            rb.velocity = -direction * (moveSpeed * 0.3f);
        }
        else
        {
            rb.velocity = Vector2.zero;
            
        }
    }

    private void ShootProjectile()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle-90f);;
        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, rotation);
        
        Rigidbody2D projRb2D = projectileObj.GetComponent<Rigidbody2D>();

        attackTimer = attackCooldown;
        if(projRb2D != null)
        {
            projRb2D.velocity = direction * projectileSpeed;
        }
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;
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
}
