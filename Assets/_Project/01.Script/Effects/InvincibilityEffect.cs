
using System.Collections;
using UnityEngine;

public class InvincibilityEffect : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private float blinkSpeed = 0.1f;
    
    private Coroutine blinkCoroutine;
    private bool isBlinking = false;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        originalColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }
    }


    public void StartBlinking()
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            Debug.LogWarning("SpriteRenderers not initialized!");
            return;
        }

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        isBlinking = true;
        blinkCoroutine = StartCoroutine(BlinkEffect());
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        isBlinking = false;
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if( i < originalColors.Length && spriteRenderers[i] != null)
            {
                spriteRenderers[i].color = originalColors[i];
            }
        }
    }

    private IEnumerator BlinkEffect()
    {
        // 0.2초 대기 (피격 이펙트)
        yield return new WaitForSeconds(0.2f);

        // while(true) 대신 isBlinking 사용
        while (isBlinking)
        {
            SetAlpha(minAlpha);
            yield return new WaitForSeconds(blinkSpeed);

            SetAlpha(1f);
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderers == null) return;
        
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if(spriteRenderers[i] != null)
            {
                Color color = spriteRenderers[i].color;
                color.a = alpha;
                spriteRenderers[i].color = color;
            }
        }
    }
}
