using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // 다른클레스에서 알아야하는 bool을 매개변수로 받는 이벤트선언
    public event Action<bool> switchButtionboolChanged;

    // 실질적 스위치 On,Off 구문해줄 bool
    private bool isSwitchOn = true;

    // 스위치 바꿀떄 잠시만 true로 해서 과도한 이벤트 발생 안시키기위한 bool 변수
    public bool isTouchButton = false;

    public bool IsSwitchOn
    {        
        get {  return isSwitchOn; }
        set
        {
            //Debug.Log("Set에 들어가나?");
            if (isSwitchOn != value) // && isTouchButton == true && switchControlNum == 1
            {
                //Debug.Log("이벤트에 들어오나?");
                isSwitchOn = value;
                // 이벤트 호출 및 매개변수 전달
                switchButtionboolChanged?.Invoke(isSwitchOn);
            }
            else { /*PASS*/ }
        }
    }

    private int switchControlNum = 0;

    public SpriteRenderer switchSpriteRenderer;

    public Sprite[] switchSprite = new Sprite[2];

    // 코루틴 캐싱
    private Coroutine touchButton;

    private WaitForFixedUpdate waitForFixedUpdate = default;

    public void Awake()
    {
        //isSwitchOn = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //switchSprite = new Sprite[direction];
        switchSpriteRenderer = GetComponent<SpriteRenderer>();

        if(waitForFixedUpdate == default || waitForFixedUpdate == null)
        {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }
        else { /*PASS*/ }



    }

    // Update is called once per frame
    void Update()
    {
        ChangeSwitch();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switchControlNum = 1;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switchControlNum = 0;
        }
    }

    public void ChangeSwitch()
    {
        if (switchControlNum == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                
                //  { 스페이스 입력하면 스위치 온,오프 조정
                if (IsSwitchOn == true && isTouchButton == false)
                {
                    switchSpriteRenderer.sprite = switchSprite[0];
                    //Debug.LogFormat("값 : {0}", switchSpriteRenderer.sprite);
                    IsSwitchOn = false;
                    touchButton = StartCoroutine(ChangeTouchSwitch());

                }
                else if (IsSwitchOn == false && isTouchButton == false)
                {
                    
                    switchSpriteRenderer.sprite = switchSprite[1];
                    //Debug.LogFormat("값 : {0}", switchSpriteRenderer.sprite);
                    IsSwitchOn = true;
                    touchButton = StartCoroutine(ChangeTouchSwitch());
                }
                Debug.LogFormat("IsSwitchOn 값 : {0}", IsSwitchOn);
                //  { 스페이스 입력하면 스위치 온,오프 조정
            }
            else { /*PASS*/ }
        }
    }


    // 터치했을때에 잠깐만 True로 두고 false로 바꿔줄 코루틴
    IEnumerator ChangeTouchSwitch()
    {
        isTouchButton = true;
        for(int i =0; i <= 5; i++) { yield return waitForFixedUpdate; }
        isTouchButton = false;
    }


}   // NameSpace
