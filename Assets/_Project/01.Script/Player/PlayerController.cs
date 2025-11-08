using UnityEngine;

/// <summary>
/// 플레이어 이동을 담당하는 컨트롤러
/// WASD 또는 방향키로 8방향 이동
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("컴포넌트")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    

    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleInput();
        //UpdateAnimation();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(horizontal, vertical).normalized;
        Vector2 animInput = new Vector2(horizontal, vertical);
        
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
        
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = animInput;
        }

        HandleAnimation(animInput);
    }

    private void HandleAnimation(Vector2 moveDir)
    {
        float speed = moveDir.magnitude;
        animator.SetFloat("Speed", speed);

        if (speed > 0)
        {
            if (moveDir.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (moveDir.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            
            animator.SetFloat("horizontal", moveDir.x);
            animator.SetFloat("vertical", moveDir.y);
            
            lastMoveDirection = moveDir; 
        }
        else
        {
            animator.SetFloat("horizontal", lastMoveDirection.x);
            animator.SetFloat("vertical", lastMoveDirection.y);
        }
        
    }

    void Move()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
        }

        if (spriteRenderer != null && moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}