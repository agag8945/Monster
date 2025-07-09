using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 이동 속도
    [SerializeField]
    float moveSpeed = 1f;

    public float playerHp = 3;

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

    // 애니메이터 컴포넌트 참조
    private Animator animator;
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

        Shoot();
    }

    void Shoot()
    {
        if (missIndex < 0 || missIndex >= missilePrefab.Length) return;
        if (Time.time - lastshotTime > shootInverval)
        {
            Instantiate(missilePrefab[missIndex], spPosition.position, Quaternion.identity);
            lastshotTime = Time.time;
        }
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

    public void TakeDamage(int damage)
    {
        playerHp -= damage;

        if (playerHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.GameOver();
    }
}
