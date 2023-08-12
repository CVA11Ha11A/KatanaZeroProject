using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SG_RedDottedLineControler : MonoBehaviour
{
    public GameObject shotLaser;

    public GameObject dotted001;
    public GameObject dotted002;
    public GameObject dotted003;
    public GameObject dotted004;

    private Switch switchClass;

    private bool redDottedIsButtonSwitch = default;
    

    private float onOffDotted = 0f;
    private float dottedSpeed = 2f;
    private int dottedcontrolNum = 0;


    //private bool isLaserShot = false;
    //public event Action<bool> isLaserShotEvent;
    //public bool IsLaserShot
    //{
    //    get { return isLaserShot; }

    //    set
    //    {
    //        if (isLaserShot != value) // && isTouchButton == true && switchControlNum == 1
    //        {
    //           
    //            isLaserShot = value;
    //            // 이벤트 호출 및 매개변수 전달
    //            isLaserShotEvent?.Invoke(isLaserShot);
    //        }
    //        else { /*PASS*/ }
    //    }
    //}


    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    void Start()
    {
        if(redDottedIsButtonSwitch == default)
        {
            redDottedIsButtonSwitch = true;
        }

        switchClass = FindAnyObjectByType<Switch>();
        switchClass.switchButtionboolChanged += RedDotteLineIsSwitchOn;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogFormat("redDottedIsButtonSwitch 값 ->{0}", redDottedIsButtonSwitch);
        if (redDottedIsButtonSwitch == true)
        {
            onOffDotted += dottedSpeed * Time.deltaTime;
            //Debug.LogFormat("Dotted -> {0}", onOffDotted);

            if (onOffDotted >= 0.08f)
            {
                DottedLineControls();
            }

            else { /*PASS*/ }
        }
    }

    void OnEnable()
    {

        // SetActive == true 될 때 수행할 작업
        Debug.Log("빨간점선 Active 켜짐");
    }

    void OnDisable()
    {
        // SetActive == flase 될 때 수행할 작업

        if (redDottedIsButtonSwitch == true)
        { 
            shotLaser.gameObject.SetActive(true);
        }
        Debug.Log("빨간점선 Active 꺼짐");
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {

        //if (redDottedIsButtonSwitch == true)
        //{
            if (collision.gameObject.CompareTag("Player"))
            {                
                this.gameObject.SetActive(false);
                //잘들어옴
                //Debug.Log("SetActive(false) 이곳에 들어오는지"); 
            }
        //}

    }


    private void DottedLineControls()
    {

        if (dottedcontrolNum == 0)
        {
            onOffDotted = 0;
            dotted001.SetActive(true);
            dotted002.SetActive(false);
            dotted003.SetActive(false);
            dotted004.SetActive(false);
            dottedcontrolNum = 1;
        }
        else if (dottedcontrolNum == 1)
        {
            onOffDotted = 0;
            dotted001.SetActive(false);
            dotted002.SetActive(true);
            dotted003.SetActive(false);
            dotted004.SetActive(false);
            dottedcontrolNum = 2;
        }
        else if (dottedcontrolNum == 2)
        {
            onOffDotted = 0;
            dotted001.SetActive(false);
            dotted002.SetActive(false);
            dotted003.SetActive(true);
            dotted004.SetActive(false);
            dottedcontrolNum = 3;
        }
        else if (dottedcontrolNum == 3)
        {
            onOffDotted = 0;
            dotted001.SetActive(false);
            dotted002.SetActive(false);
            dotted003.SetActive(false);
            dotted004.SetActive(true);
            dottedcontrolNum = 0;
        }


    }

    private void RedDotteLineIsSwitchOn(bool buttonSwitch)
    {
        redDottedIsButtonSwitch = buttonSwitch;
    }

}
