using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlowMusic : MonoBehaviour
{
    AudioSource bgm;
    TimeManager timeManager;
    Kissyface_manager kissyface;

    private static SlowMusic _instance;

    // 다른 스크립트에서 TimeManager에 접근할 때 사용할 정적 인스턴스
    public static SlowMusic Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SlowMusic>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // 이미 다른 씬에서 생성된 인스턴스가 있을 경우 이 인스턴스를 파괴합니다.
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _instance = this;
    }
    void Start()
    {
        timeManager = FindAnyObjectByType<TimeManager>();
        bgm = GetComponent<AudioSource>();
        kissyface = FindAnyObjectByType<Kissyface_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name!= "BossScene")
        {
            Destroy(gameObject);
        }
        if(kissyface.isDie)
        {
            Destroy(gameObject);
        }
        if(timeManager.isTimeSlow==true)
        {
            bgm.pitch = 0.5f;
        }
        else if(timeManager.isTimeSlow==false)
        {
            bgm.pitch = 1;
        }

    }
}
