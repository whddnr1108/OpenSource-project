using System.Collections;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 15;         // 최대 체력
    private float currentHP;          // 현재 체력

    [SerializeField]
    private Sprite normalSprite;       // 플레이어의 평소 스프라이트
    [SerializeField]
    private Sprite shieldSprite;       // 쉴드 아이템 효과 스프라이트

    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    public float MaxHP => maxHP;              // maxHP 변수에 접근할 수 있는 프로퍼티 (Get만 가능)
    public float CurrentHP                    // currentHP 변수에 접근할 수 있는 프로퍼티 (Set, Get 가능)
    {
        set => currentHP = Mathf.Clamp(value, 0, maxHP);
        get => currentHP;
    }

    // 쉴드 활성화 여부
    public bool HasShield { get; private set; } = false;
    private Coroutine shieldCoroutineInstance; // 쉴드 코루틴 참조 변수 (중복 실행 방지용)

    private void Awake()
    {
        currentHP = maxHP;             // 현재 체력을 최대 체력과 같게 설정
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();

        // SpriteRenderer 컴포넌트가 없으면 경고 출력
        if (spriteRenderer == null)
        {
            Debug.LogError("PlayerHP: SpriteRenderer component not found on this GameObject!", this);
        }
        // 초기 스프라이트를 normalSprite로 설정 (Inspector에서 설정되어있다면 생략 가능)
        if (spriteRenderer != null && normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    public void TakeDamage(float damage)
    {
        if (HasShield)
        {
            Debug.Log("Player has shield, damage absorbed!");
            return; // 쉴드가 있으면 데미지를 받지 않음
        }

        // 현재 체력을 damage만큼 감소
        currentHP -= damage;

        // 체력 감소 시 잠시 빨간색으로 깜빡이는 효과
        StopCoroutine("HitColorAnimation"); // 기존에 실행 중인 코루틴 중단
        StartCoroutine("HitColorAnimation"); // 새로운 코루틴 시작

        // 체력이 0이하 = 플레이어 캐릭터 사망
        if (currentHP <= 0)
        {
            currentHP = 0; // 체력이 음수가 되는 것을 방지
            Debug.Log("Player died!");
            // 체력이 0이면 OnDie() 함수를 호출해서 죽었을 때 처리를 한다
            playerController.OnDie(); // PlayerController 스크립트에 OnDie() 함수가 구현되어 있어야 함
        }
        Debug.Log($"Player HP: {currentHP}/{maxHP}");
    }

    private IEnumerator HitColorAnimation()
    {
        // 플레이어의 색상을 빨간색으로
        spriteRenderer.color = Color.red;
        // 0.1초 동안 대기
        yield return new WaitForSeconds(0.1f);
        // 플레이어의 색상을 원래 색상인 하얀색으로
        spriteRenderer.color = Color.white;
    }

    public void GiveShield()
    {
        if (!HasShield) // 쉴드가 활성화되어 있지 않을 때만 실행
        {
            // 이전에 실행 중이던 쉴드 코루틴이 있다면 중단 (중복 방지 및 타이머 리셋)
            if (shieldCoroutineInstance != null)
            {
                StopCoroutine(shieldCoroutineInstance);
            }
            Debug.Log("Shield activated!");
            shieldCoroutineInstance = StartCoroutine(ShieldCoroutine());
        }
        else
        {
            Debug.Log("Shield already active, resetting duration.");
            // 이미 쉴드가 있다면 시간만 리셋 (다시 코루틴 시작)
            if (shieldCoroutineInstance != null)
            {
                StopCoroutine(shieldCoroutineInstance);
            }
            shieldCoroutineInstance = StartCoroutine(ShieldCoroutine());
        }
    }

    private IEnumerator ShieldCoroutine()
    {
        HasShield = true;
        UpdateShieldVisual(); // 쉴드 스프라이트로 변경
        yield return new WaitForSeconds(3f); // 3초 대기
        HasShield = false;
        UpdateShieldVisual(); // 일반 스프라이트로 변경
        shieldCoroutineInstance = null; // 코루틴이 완료되었음을 표시
        Debug.Log("Shield deactivated.");
    }

    // 쉴드 활성화 여부에 따라 스프라이트를 업데이트하는 함수
    private void UpdateShieldVisual()
    {
        if (spriteRenderer != null)
        {
            // HasShield가 true면 shieldSprite, false면 normalSprite 적용
            spriteRenderer.sprite = HasShield ? shieldSprite : normalSprite;
        }
    }

    // 아이템 획득 예시 (다른 스크립트에서 호출될 수 있음)
    // 예를 들어, 플레이어의 Collider에 닿았을 때 아이템 스크립트에서 이 함수를 호출
    // public void OnItemCollected(string itemType)
    // {
    //     if (itemType == "Shield")
    //     {
    //         GiveShield();
    //     }
    // }
}