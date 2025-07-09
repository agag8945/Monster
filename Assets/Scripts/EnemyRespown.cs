using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespown : MonoBehaviour
{
    // 적 프리팹 배열
    public GameObject[] Enemies;

    // 적이 생성될 좌표 배열
    float[] arrPosX = { -2f, 0f, 2f };

    [SerializeField]
    float spawnInterval = 0.5f; // 적 생성 간격

    float moveSpeed = 5f; // 적 이동 속도

    // 적 생성 위치 (Transform)
    public Transform spawnPosition;

    int currentEnemyIndex = 0; // 현재 적 인덱스
    int spawnCount = 0;        // 생성 횟수

    void Start()
    {
        StartCoroutine("EnemyRoutine"); // 적 생성 코루틴 시작
    }

    // 적을 주기적으로 생성하는 코루틴
    IEnumerator EnemyRoutine()
    {
        yield return new WaitForSeconds(3); // 게임 시작 후 3초 대기

        while (true)
        {
            for (int i = 0; i < arrPosX.Length; i++)
            {
                SpawnEnemy(arrPosX[i], currentEnemyIndex, moveSpeed); // 각 위치에 적 생성
                spawnCount++; // 생성 횟수 증가
            }

            // 2회마다 적 종류와 속도 증가
            if (spawnCount % 2 == 0)
            {
                currentEnemyIndex++;
                if (currentEnemyIndex >= Enemies.Length)
                {
                    currentEnemyIndex = Enemies.Length - 1; // 인덱스 범위 제한
                }
                moveSpeed += 2; // 속도 증가
            }

            yield return new WaitForSeconds(spawnInterval); // 다음 생성까지 대기
        }
    }

    // 적을 생성하는 함수
    void SpawnEnemy(float posX, int index, float moveSpeed)
    {
        Vector3 spawnPos = new Vector3(posX, spawnPosition.position.y, spawnPosition.position.z);

        GameObject enemyObject = Instantiate(Enemies[index], spawnPos, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();

        enemy.SetMoveSpeed(moveSpeed); // 이동 속도 설정
    }
}
