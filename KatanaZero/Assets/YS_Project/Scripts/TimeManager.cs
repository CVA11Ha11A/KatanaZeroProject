using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class TimeManager : MonoBehaviour
{
    Player player;
    public Slider timerSlider;
    private float timeMax = 90f;
    private float currentTime;
    private bool isTimeSlow=false;
    WaitForSeconds oneSeconds = new WaitForSeconds(1);
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        currentTime = timeMax;
        StartCoroutine(StageTimer());
        
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
        }

        if(isTimeSlow)
        {
            Time.timeScale = 0.4f;
        }
        else
        {
            Time.timeScale = 1;
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
}
