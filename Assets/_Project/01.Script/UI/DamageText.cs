using TMPro;
using UnityEngine;

namespace _Project._01.Script.UI
{
    public class DamageText : MonoBehaviour
    {
        [Header("설정")]
        [SerializeField] private float moveSpeed = 1f;      // 위로 올라가는 속도 (월드 유닛)
        [SerializeField] private float lifetime = 2f;       // 생존 시간

        private TextMeshProUGUI text;
        private RectTransform rectTransform;
        private Color originalColor;
        private float timer;

        // 월드 위치 추적용
        private Vector3 worldPosition;
        private Canvas parentCanvas;
        private Camera mainCamera;

        void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
            originalColor = text.color;
            mainCamera = Camera.main;
        }

        public void Initialize(Vector3 startWorldPosition, Canvas canvas)
        {
            worldPosition = startWorldPosition;
            parentCanvas = canvas;
        }

        void Update()
        {
            // 월드 위치를 위로 이동
            worldPosition += Vector3.up * moveSpeed * Time.deltaTime;

            // 월드 좌표 → 스크린 좌표 → Canvas 로컬 좌표로 변환
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);

            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.GetComponent<RectTransform>(),
                screenPos,
                parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                out canvasPos
            );

            rectTransform.anchoredPosition = canvasPos;
        
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
}
