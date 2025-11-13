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
        
        originalAttackCooldown = bossAI.attackCooldown;
        originalColor = spriteRenderer.color;

        if (bossHealth != null)
        {
            bossHealth.OnPhase2 += EnterPhase2;
        }
    }

    void EnterPhase2()
    {
        Debug.Log("Enter Phase 2");
        currentPhase = BossPhase.Phase2;

        bossAI.attackCooldown = phase2AttackCooldown;
        Debug.Log($"공격 쿨타임: {originalAttackCooldown} → {phase2AttackCooldown}");
    
        // 색상 변경
        Debug.Log($"SpriteRenderer: {spriteRenderer != null}");  // null 체크
        Debug.Log($"원래 색: {originalColor}");
        spriteRenderer.color = phase2Color;
        Debug.Log($"바뀐 색: {spriteRenderer.color}");
    }

    private void OnDestroy()
    {
        if (bossHealth != null)
        {
            bossHealth.OnPhase2 -= EnterPhase2;
        }
        if(bossHealth.GetCurrentHealth() <= 0)
        {
            WaveUIManager waveUI = FindObjectOfType<WaveUIManager>();
            waveUI.ShowVictory();
        }
    }
}
