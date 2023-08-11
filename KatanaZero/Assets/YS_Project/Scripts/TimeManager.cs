using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class TimeManager : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
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
