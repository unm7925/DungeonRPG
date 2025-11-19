using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossPhase
    {
        Phase1,
        Phase2
    }
    
    [Header("Current State")]
    [SerializeField] private BossPhase currentPhase =  BossPhase.Phase1;
    
    [Header("References")]
    [SerializeField] private EnemyHealth bossHealth;
    [SerializeField] private BossAI bossAI;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Phase 2 Settings")] 
    [SerializeField] private float phase2AttackCooldown = 0.5f;
    [SerializeField] private Color phase2Color = Color.red;

    public BossPhase CurrentPhase => currentPhase;
    private float originalAttackCooldown;
    private Color originalColor;

    private void Start()
    {
        bossAI = GetComponent<BossAI>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bossHealth = GetComponent<EnemyHealth>();
        
        if (bossAI != null)
        {
            originalAttackCooldown = bossAI.attackCooldown;
        }
        if(spriteRenderer!=null)
        {
            originalColor = spriteRenderer.color;
        }
        if (bossHealth != null)
        {
            bossHealth.OnPhase2 += EnterPhase2;
            bossHealth.OnDeath += OnBossDeath;
        }
    }

    private void OnBossDeath()
    {
        GameOverManager.Instance?.ShowVictory();
    }

    void EnterPhase2()
    {
        // 페이즈 2 돌입
        currentPhase = BossPhase.Phase2;
        
        // 공격 쿨타임 변경
        if(bossAI != null)
        {
            bossAI.attackCooldown = phase2AttackCooldown;
        }
        // 색상 변경
        if(spriteRenderer != null)
        {
            spriteRenderer.color = phase2Color;
        }
    }

    private void OnDestroy()
    {
        if (bossHealth != null)
        {
            bossHealth.OnPhase2 -= EnterPhase2;
            bossHealth.OnDeath -= OnBossDeath;
        }
    }
}
