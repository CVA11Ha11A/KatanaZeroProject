using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using Rewired.Components;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;

    // 다른 스크립트에서 TimeManager에 접근할 때 사용할 정적 인스턴스
    public static TimeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeManager>();
            }
            return _instance;
        }   
    }
    //private void Awake()
    //{
    //    if (_instance != null && _instance != this)
    //    {
    //        // 이미 다른 씬에서 생성된 인스턴스가 있을 경우 이 인스턴스를 파괴합니다.
    //        Destroy(this.gameObject);
    //        return;
    //    }

    //    DontDestroyOnLoad(gameObject);
    //    _instance = this;
    //}

    Player player;
    public Slider timerSlider;
    public GameObject slowLight;
    public GameObject[] batterys;
    public GameObject globalLight;
    public bool isTimeSlow=false;
    private float timeMax = 90f;
    private float currentTime;
    private float batteryMax=11;
    private float currentBattery;
    private bool isBatteryUse = false;
    private bool isBatteryCharge = false;
    WaitForSeconds oneSeconds = new WaitForSeconds(1);
    WaitForSeconds halfSeconds = new WaitForSeconds(0.25f);
    PlayerMove playerMove;
    SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        playerMove = FindAnyObjectByType<PlayerMove>();
        player = ReInput.players.GetPlayer(0);
        
        
    }
    private void OnEnable()
    {
        currentTime = timeMax;
        StartCoroutine(StageTimer());
        currentBattery = batteryMax;
    }

    // Update is called once per frame
    void Update()
    {
        timerSlider.value = currentTime / timeMax;
        if(currentTime/timeMax<=0)
        {
            playerMove.Die();
        }
        if(player.GetButtonDown("SlowTime"))
        {
           
        }
        if(player.GetButton("SlowTime"))
        {
            isTimeSlow = true;
            
        }
        else if(player.GetButtonUp("SlowTime"))
        {
            isTimeSlow = false;
            StopCoroutine(UseBattery());
        }

        if(isTimeSlow&&currentBattery>0)
        {
            slowLight.SetActive(true);
            globalLight.SetActive(false);

            if (isBatteryUse==false)
            {
            StartCoroutine(UseBattery());

            }
            Time.timeScale = 0.4f;
        }
        else
        {

            slowLight.SetActive(false);
            globalLight.SetActive(true);
            Time.timeScale = 1;
            if(isBatteryCharge==false)
            {
            StartCoroutine(ChargeBattery());

            }
        }
    }
    private IEnumerator StageTimer()
    {
        while(currentTime>0)
        {

        yield return oneSeconds;
        currentTime -= 1;
        }


    }
    private IEnumerator UseBattery()
    {
        isBatteryUse = true;
        while(currentBattery>0)
        {
            if(isTimeSlow==false)
            {
               break;
            }
        batterys[(int)currentBattery-1].SetActive(false);
        currentBattery -= 1;
        yield return halfSeconds;

        }
        isBatteryUse = false;
    }
    private IEnumerator ChargeBattery()
    {
        isBatteryCharge = true;
        while(currentBattery<11)
        {
            if(isTimeSlow==true)
            {
                break;
            }
            currentBattery += 1;
        batterys[(int)currentBattery-1].SetActive(true);
        yield return oneSeconds;

        }
        isBatteryCharge = false;
    }
}
