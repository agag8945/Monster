using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    // 플레이어 이동 속도
    [SerializeField]
    float moveSpeed = 1f;

    public int playerHp = 3;

    // 현재 미사일 프리팹 인덱스 
    int missIndex = 0;

    //미사일 프리팹 배열
    public GameObject[] missilePrefab;

    // 미사일 생성 위치
    public Transform spPosition;

    // 미사일 발사 간격(초)
    [SerializeField]
    private float shootInverval = 0.05f;

    // 마지막 발사 시간
    private float lastshotTime = 0f;

    private bool isSpreadShot = false;
    private bool canUseSpread = true;

    [SerializeField] private UnityEngine.UI.Button spreadShotButton;
    [SerializeField] private float spreadDuration = 3f;
    [SerializeField] private float spreadCooldown = 5f;


    // 애니메이터 컴포넌트 참조
    private Animator animator;

    public GameObject[] hearts; // UI 하트 이미지 3개 연결




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // 매 프레임마다 이동 및 발사 처리
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 moveTo = new Vector3(horizontalInput, 0, 0);
        transform.position += moveTo * moveSpeed * Time.deltaTime;

        if (horizontalInput < 0)
        {
            animator.Play("Left");
        }

        else if (horizontalInput > 0)
        {
            animator.Play("Right");
        }

        else
        {
            animator.Play("Idle");
        }

        if (isSpreadShot) ShootSpread();
        
        else Shoot();

        
    }

    // 한 방향 미사일 발사
    void Shoot()
    {
        if (missIndex < 0 || missIndex >= missilePrefab.Length) return;
        if (Time.time - lastshotTime > shootInverval)
        {
            Instantiate(missilePrefab[missIndex], spPosition.position, Quaternion.identity);
            lastshotTime = Time.time;
        }
    }

    // 3방향 미사일 발사
    
    void ShootSpread()
    {
        if (missIndex < 0 || missIndex >= missilePrefab.Length) return;

        Vector3 pos = spPosition.position;

        // 방향 벡트
        Vector3[] directions = {
            Quaternion.Euler(0, 0, 15) * Vector3.up,
            Vector3.up,
            Quaternion.Euler(0, 0, -15) * Vector3.up
        };

        if (Time.time - lastshotTime > shootInverval)
        {
            foreach (var dir in directions)
            {
                GameObject missile = Instantiate(missilePrefab[missIndex], pos, Quaternion.identity);
                missile.GetComponent<Missile>().direction = dir.normalized;
            }
            lastshotTime = Time.time;
        }


    }
    

    private IEnumerator SpreadShotRoutine()
    {
        // 활성화
        canUseSpread = false;
        spreadShotButton.interactable = false;
        isSpreadShot = true;

        // 3초간 spread
        yield return new WaitForSeconds(spreadDuration);
        isSpreadShot = false;

        // 쿨타임 표시용 투명도 낮추기 (선택)
        var img = spreadShotButton.GetComponent<UnityEngine.UI.Image>();
        var tmpColor = img.color;
        tmpColor.a = 0.5f;
        img.color = tmpColor;

        // 쿨타임
        yield return new WaitForSeconds(spreadCooldown);

        // 다시 버튼 활성화
        canUseSpread = true;
        spreadShotButton.interactable = true;

        tmpColor.a = 1f;
        img.color = tmpColor;
    }

    public void MissileUp()
    {
        missIndex++;
        shootInverval = shootInverval - 0.1f;


        if (shootInverval <= 0.1f)
        {
            shootInverval = 0.1f;
        }


        if (missIndex >= missilePrefab.Length)
        {
            missIndex = missilePrefab.Length - 1;
        }
    }

    public void ActivateSpreadShot()
    {
        if (!canUseSpread) return;
        StartCoroutine(SpreadShotRoutine());
    }


    public void TakeDamage(int damage)
    {
        int prevHp = playerHp;
        playerHp -= damage;

        // 화면 진동 효과
        Camera.main.transform.DOShakePosition(0.2f, 0.2f, 20, 90, false, true);

        // 하트 깜빡이기 효과
        StartCoroutine(BlinkHeart(prevHp - 1));

        if (playerHp > 0)
        {
            for (int i = prevHp - 1; i >= playerHp && i >= 0; i--)
            {
                if (i < hearts.Length && hearts[i] != null)
                {
                    hearts[i].SetActive(false); // 인덱스 수정
                }
            }
        }

        if (playerHp <= 0)
        {
            playerHp = 0;
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i] != null)
                    hearts[i].SetActive(false); // 모든 하트 끄기
            }
            Invoke(nameof(Die), 0.2f);
        }
    }

    IEnumerator BlinkHeart(int heartIndex)
    {
        if (heartIndex < 0 || heartIndex >= hearts.Length) yield break;

        GameObject heart = hearts[heartIndex];

        if (heart == null) yield break;

        for (int i = 0; i < 3; i++)
        {
            heart.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(0.1f);
            heart.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Die()
    {
        GameManager.Instance.GameOver();
    }
}


