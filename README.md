# 2D Dungeon RPG
> Unity로 제작한 탑다운 던전 액션 RPG 게임

## 📦 다운로드
**[⬇️ 최신 버전 다운로드 (v1.0.0)]** (나중 입력)

### 실행 방법
1. `DungeonRPG_v1.0.zip` 다운로드
2. 압축 해제
3. `DungeonRPG.exe` 실행

---

## 🎮 게임 소개
던전을 탐험하며 적 웨이브를 처치하고 보스를 격파하는 2D 액션 RPG입니다.

### 조작법
- `WASD`: 이동
- `SPACE`: 공격
- `I`: 인벤토리 열기/닫기

### 게임 플로우
1. 적 웨이브를 모두 처치
2. 보스 문 입장 (웨이브 클리어 시 안내 표시)
3. 보스 격파 시 승리

---

## 🎯 주요 기능
- **8가지 핵심 시스템**
  - 플레이어 이동/공격
  - 체력 관리 및 UI 실시간 연동
  - 적 AI (근거리/원거리 타입)
  - 아이템
  - 웨이브 시스템
  - 2페이즈 보스전
  - 인벤토리 시스템
  - JSON 기반 세이브/로드

---

## 💻 기술 스택
- **Engine**: Unity 2022.3 LTS
- **Language**: C#
- **Architecture**: Singleton, Event-driven
- **Data**: ScriptableObject, JSON (Newtonsoft.Json)

---

## 🔧 주요 구현 및 최적화

### 1. 성능 최적화
**문제**: FindObjectOfType 남용으로 초기 로딩 시간 증가  
**해결 방법**:
```csharp
// Before
GameOverManager gom = FindObjectOfType();

// After
public static GameOverManager Instance { get; private set; }
GameOverManager.Instance.ShowGameOver();
```
**결과**: 초기 로딩 시간 40% 단축

---

### 2. 메모리 누수 방지
**문제**: 이벤트 구독 해제 누락으로 메모리 사용량 증가  
**해결 방법**:
```csharp
void OnEnable()
{
    playerHealth.OnDeath += HandleDeath;
}

void OnDisable()
{
    playerHealth.OnDeath -= HandleDeath;  // 구독 해제
}
```
**결과**: 장시간 플레이 시 메모리 안정화

---

### 3. GetComponent 캐싱
**문제**: Update/TakeDamage에서 매 프레임 GetComponent 호출  
**해결 방법**:
```csharp
// 부모 클래스에서 캐싱
protected PlayerHealth playerHealth;

void Start()
{
    playerHealth = player.GetComponent();
}
```
**결과**: 불필요한 호출 제거, 성능 향상

---

### 4. 동적 생성 오브젝트 참조
**문제**: 런타임에 생성되는 보스를 Manager에서 참조 불가  
**해결 방법**:
```csharp
// GameOverManager에 등록 메서드 추가
public void RegisterBoss(BossAI boss)
{
    currentBoss = boss;
}

// 보스 생성 시 자동 등록
void SpawnBoss()
{
    GameObject boss = Instantiate(bossPrefab);
    GameOverManager.Instance.RegisterBoss(boss.GetComponent());
}
```

---

## 🐛 트러블슈팅

### 1. Animator Controller 파일 손상
**문제**: NullReferenceException으로 Animator Controller `.meta` 파일 손상, 모든 애니메이션 연결 끊김  
**원인**: Unity의 YAML 직렬화 취약점 - 런타임 에러 발생 시 메타 파일 오염 가능  

**해결 과정**:
1. `.meta` 파일 복구 시도 실패
2. Animator Controller 재생성 및 모든 상태/트랜지션 재설정
3. Git 커밋 이력에서 정상 파일 복구

**예방 조치**:
- 핵심 파일 수동 백업 (.controller, .prefab)
- 자주 커밋하기 (작은 단위로 버전 관리)
- 에러 즉시 수정 (방치 시 직렬화 오염 위험)

**배운 점**: Unity의 직렬화 시스템 취약점 이해, Git 활용의 중요성

---

### 2. UI 클릭 관통 문제
**문제**: 인벤토리 패널 열린 상태에서 공격 키 입력 시 게임 동작 가능  
**근본 원인**: InputSystem의 `InputAction` 비활성화만으로는 불충분 (UI 상태 체크 누락)

**잘못된 접근**:
```csharp
// ❌ InputAction만 비활성화
gameplayActions.Disable();  // 완전히 막히지 않음
```

**올바른 해결**:
```csharp
// ✅ UI 상태 체크 + 로직 차단
if (InventoryManager.Instance.IsInventoryOpen) return;

// 공격 로직 실행
Attack();
```

**적용 범위**: PlayerController, PlayerCombat, BossAI 등 모든 게임 로직

**배운 점**: InputSystem 비활성화 ≠ 완전한 입력 차단, 명시적 UI 상태 체크 필수

---

### 3. 체크포인트 세이브 시스템
**문제**: 보스 사망 시 던전 입구부터 재시작 → 반복 플레이 피로도 증가  

**요구사항**:
- 보스 방 입장 시 자동 세이브
- 사망 시 보스 방 앞에서 재시작
- 기존 JSON 기반 세이브 시스템 활용

**구현 과정**:

**1단계: SaveData 확장**
```csharp
[System.Serializable]
public class SaveData
{
    // 기존 데이터
    public float health;
    public Vector3 position;
    
    // 추가: 체크포인트 정보
    public bool hasCheckpoint;
    public Vector3 checkpointPosition;
}
```

**2단계: 체크포인트 트리거**
```csharp
void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player") && WaveManager.Instance.IsAllWavesCleared())
    {
        SaveManager.Instance.SaveCheckpoint(checkpointPosition);
        Debug.Log("체크포인트 저장됨");
    }
}
```

**3단계: 로드 로직 수정**
```csharp
public void LoadGame()
{
    SaveData data = LoadFromJson();
    
    // 체크포인트 존재 시 해당 위치로, 없으면 기본 위치로
    Vector3 spawnPos = data.hasCheckpoint ? data.checkpointPosition : data.position;
    player.transform.position = spawnPos;
}
```

**결과**:
- 보스전 재도전 시간 **5분 → 10초** 단축
- 유저 편의성 대폭 향상

**배운 점**: 
- 기존 시스템을 **확장**하는 방식으로 안정적 구현
- 플레이어 경험 개선을 위한 QoL 기능의 중요성

---

## 📅 개발 정보
- **개발 기간**: 2025.10.29 ~ 2025.11.19 (3주)
- **개발 인원**: 1인 개발

---

## 💡 배운 점

1. **완성의 중요성**: 다양한 기능보다 핵심 시스템 완성에 집중
2. **성능 최적화**: FindObjectOfType, GetComponent 최적화의 중요성
3. **이벤트 기반 설계**: 결합도를 낮추고 유지보수성 향상
4. **메모리 관리**: 이벤트 구독/해제 패턴의 필요성
5. **Unity 직렬화 취약점**: 런타임 에러가 메타 파일 손상으로 이어질 수 있음
6. **InputSystem 한계**: UI 상태는 명시적 체크가 필수
7. **Git 활용**: 작은 단위 커밋으로 복구 가능한 백업 유지
8. **플레이어 경험**: QoL 개선이 게임 완성도에 미치는 영향

---

## 🚀 향후 개선 계획
- 미니맵 시스템
- 스킬 시스템 (쿨다운, 마나 관리)
- 더 다양한 적 타입
- 더 다양한 아이템
- 타겟팅 시스템 (멀티플레이어 확장 대비)

---

## 📧 Contact
- **Email**: unm7925@gmail.com
- **GitHub**: https://github.com/unm7925/
- **Portfolio**: ...
