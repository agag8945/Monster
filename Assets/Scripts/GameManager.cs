using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 현재 코인 개수
    public int coin = 0;

    // 코인 개수를 표시할 텍스트
    public TextMeshProUGUI textMeshProCoin;

    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        // 싱글톤 인스턴스가 없으면 자신을 할당
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 중복 파괴
        }
    }

    // 코인 개수를 증가시키고 UI에 표시, 2개마다 미사일 업그레이드
    public void ShowCoinCount()
    {
        coin++;
        textMeshProCoin.SetText(coin.ToString()); // 코인 수 UI 갱신

        // 코인이 2의 배수일 때마다 미사일 업그레이드
        if (coin % 2 == 0)
        {
            Player player = FindFirstObjectByType<Player>();
            if (player != null)
            {
                player.MissileUp(); // 2개마다 미사일 업그레이드
            }
        }
    }
}
