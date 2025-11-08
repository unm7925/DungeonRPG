using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    [Header("보스 전용")] 
    [SerializeField] private float chargeSpeed = 5f;
    [SerializeField] private float chargeDuration = 1f;
    [SerializeField] private float specialAttackCooldown = 10f;
    [SerializeField] private GameObject effect_Rush;
    [SerializeField] private GameObject effect_Warning;
    [SerializeField] private GameObject effect_Explosion;


    private BossController bossController;
    private float specailTimer;
    private bool isCharging = false;
    protected override void Start()
    {
        base.Start();
        specailTimer = specialAttackCooldown;
        bossController = GetComponent<BossController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(!isCharging)
        {
            base.Update();
        }
        
        specailTimer -= Time.deltaTime;
        if(bossController.CurrentPhase == BossController.BossPhase.Phase1)
        {
            if (specailTimer <= 0 && currentState == State.Chase && !isCharging)
            {
                StartCoroutine(ChargeAttackCoroutine());
                specailTimer = specialAttackCooldown;
            }
        }
        else if (bossController.CurrentPhase == BossController.BossPhase.Phase2)
        {
            if (specailTimer <= 0 && currentState == State.Chase && !isCharging)
            {
                int random = Random.Range(0, 3);

                if (random == 0)
                {
                    StartCoroutine(ChargeAttackCoroutine());
                }
                else if (random == 1)
                {
                    StartCoroutine(AreaAttackCoroutine());
                }
                else
                {
                    Debug.Log("boom");
                }

                specailTimer = specialAttackCooldown * 0.7f;
            }
        }
    }

    private IEnumerator ChargeAttackCoroutine()
    {
        isCharging = true;
        GameObject effect = Instantiate(effect_Rush, transform.position, Quaternion.identity,transform);
        Destroy(effect, 1f);
        Vector2 direction = (player.position - transform.position).normalized;
        float timer = 0f;
        bool hasHitPlayer = false;
        
        while (timer < chargeDuration)
        {
            // 벽 체크
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, LayerMask.GetMask("Wall"));
            
            if (hit.collider != null)
            {
                // 벽에 부딪히면 스턴 (선택사항)
                rb.velocity = Vector2.zero;
                yield return new WaitForSeconds(0.5f); // 0.5초 멈춤
                break;
            }

            if (!hasHitPlayer)
            {
                RaycastHit2D hit_Player = Physics2D.Raycast(transform.position, direction, 1f, LayerMask.GetMask("Player"));
                if (hit_Player.collider != null)
                {
                    PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(attackDamage * 2);
                        Debug.Log("Charge Attack");
                        hasHitPlayer = true;
                    }
                }
            }
        
            rb.velocity = direction * chargeSpeed;
            timer += Time.deltaTime;
            yield return null;
        }
        
        // 돌진 끝
        rb.velocity = Vector2.zero;
        isCharging = false;
    }

    private IEnumerator AreaAttackCoroutine()
    {
        isCharging = true;
        rb.velocity = Vector2.zero;
        GameObject _effect_Warning = Instantiate(effect_Warning, transform.position, Quaternion.identity);
        Destroy(_effect_Warning, 0.5f);
        yield return new WaitForSeconds(1f);
        
        GameObject _effect_Explosion = Instantiate(effect_Explosion,transform.position,Quaternion.identity);
        Destroy(_effect_Explosion,1f);
        
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,5f,LayerMask.GetMask("Player"));

        foreach (var hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage * 3);
                Debug.Log("Charge Attack");
            }
        }

        isCharging = false;
    }
}
