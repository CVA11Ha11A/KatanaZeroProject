using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        // GameManager 인스턴스가 이미 있는 경우 새로 생성하지 않고 기존 인스턴스를 사용합니다.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지되도록 설정
        }
        else
        {
            // 이미 인스턴스가 존재하면 현재 게임 오브젝트를 파괴합니다.
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        
            UnityEngine.Cursor.visible = false;
        
    }
}
