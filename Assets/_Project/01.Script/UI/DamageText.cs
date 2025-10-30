using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float moveSpeed = 50f;      // 위로 올라가는 속도
    //[SerializeField] private float fadeSpeed = 1f;      // 사라지는 속도
    [SerializeField] private float lifetime = 1f;       // 생존 시간
    
    private Text text;
    private Color originalColor;
    private float timer;
    
    void Awake()
    {
        text = GetComponent<Text>();
        originalColor = text.color;
    }
    
    void Update()
    {
        // 위로 이동
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        
        // 페이드 아웃
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
        
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        
        // 시간 지나면 삭제
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    
    // 데미지 값 설정
    public void SetDamage(int damage)
    {
        text.text = $"-{damage}";
    }
}
