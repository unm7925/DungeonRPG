using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;

    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    private void Start()
    {
        Debug.Log("HitEffect Start 호출됨!");  // ← 추가
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers != null)
        {
            originalColors = new Color[spriteRenderers.Length];

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                originalColors[i] = spriteRenderers[i].color;
            }
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found!");   
        }
        
    }

    public void PlayHitEffect()
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0) return;

        StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        Color[] currentColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            currentColors[i] = spriteRenderers[i].color;
        }
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = hitColor;
        }
        
        // 2. flashDuration 시간 대기
        yield return new WaitForSeconds(flashDuration);
        
        // 3. 모든 스프라이트를 원래 색상으로 복구
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = currentColors[i];
        }
    }
}
