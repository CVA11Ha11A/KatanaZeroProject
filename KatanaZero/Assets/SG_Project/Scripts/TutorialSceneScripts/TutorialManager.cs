using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TutorialManager : MonoBehaviour
{
    TutorialPlayerFirstControlScript tutorialPlayer;

    TutorialGunEnemyScript tutorialEnemy;

    // { LEGACY : 뒷배경
    public GameObject timeBackGround;
    SpriteRenderer timeBackGroundRenderer;
    Color timeBackGroundColor;
    // } LEGACY : 뒷배경

    public GameObject backGroundLight;
    public GameObject playerLight;
    public GameObject enemyLight;
    public Light2D globalLight;

    Coroutine coroutineBoxing;
    WaitForFixedUpdate waitForFixed;
    WaitForSeconds waitForSeconds;

    bool enemyShotEventTime = false;
    bool didTimeScaleEvent = false;


    float backGroundRgb_A = 4;

    
    void Start()
    {
        FirstInIt();
        tutorialEnemy.enemyShotEvent += EnemyShotBoolEvent;
        //globalLight = GetComponent<Light2D>();
        globalLight = backGroundLight.GetComponent<Light2D>();

    }

    // Update is called once per frame
    void Update()
    {
        TimeSclaseEvent();
    }

    private void FirstInIt()
    {
        if (tutorialEnemy == default || tutorialEnemy == null)
        {
            tutorialEnemy = FindObjectOfType<TutorialGunEnemyScript>();
        }
        else { /*PASS*/ }

        if (tutorialPlayer == default || tutorialPlayer == null)
        {
            tutorialPlayer = GetComponent<TutorialPlayerFirstControlScript>();
        }
        else { /*PASS*/ }

        if(waitForSeconds == default || waitForSeconds == null)
        {
            waitForSeconds = new WaitForSeconds(0.1f);
        }

        timeBackGroundRenderer = timeBackGround.GetComponent<SpriteRenderer>();
    }

    public void EnemyShotBoolEvent(bool enemyShot)
    {
        enemyShotEventTime = enemyShot;
        //Debug.LogFormat("이벤트 실행후 bool값 -> {0}", enemyShotEventTime);
    }

    public void TimeSclaseEvent()
    {

        if(didTimeScaleEvent == false && enemyShotEventTime == true)
        {
            didTimeScaleEvent = true;
            coroutineBoxing = StartCoroutine(TimeCoroutine());
        }

    }



    public IEnumerator TimeCoroutine()
    {
        enemyLight.SetActive(true);
        playerLight.SetActive(true);
        
        for(int i = 0; i <= 5; i++)
        {
            Time.timeScale -= 0.19f;

            for(int j =0; j <= 10; j++)
            {
                timeBackGroundColor = globalLight.color;

                timeBackGroundColor.r = timeBackGroundColor.r - backGroundRgb_A;
                timeBackGroundColor.g = timeBackGroundColor.r - backGroundRgb_A;
                timeBackGroundColor.b = timeBackGroundColor.r - backGroundRgb_A;

                globalLight.color = timeBackGroundColor;

            }
            #region LEGACY : 뒷배경 SpareObj 조정
            //for (int j = 0; j <= 10; j++)
            //{
            //    timeBackGroundColor = timeBackGroundRenderer.color;
            //    //Debug.LogFormat("{0}", timeBackGroundRenderer.color);
            //    timeBackGroundColor.a += backGroundRgb_A;
            //    timeBackGroundRenderer.color = timeBackGroundColor;
            //}
            #endregion LEGACY : 뒷배경 SpareObj 조정
            yield return waitForSeconds;
            
        }


        Time.timeScale = 0f;


    }

}
