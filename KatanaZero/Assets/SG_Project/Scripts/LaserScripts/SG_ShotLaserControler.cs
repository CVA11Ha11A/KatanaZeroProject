using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SG_ShotLaserControler : MonoBehaviour
{
    // 플레이어랑 닿았을때에 플레이어 사망처리를 위한 Class참조
    public SG_PlayerMovement player = default;

    public GameObject dottedLine;

    private Switch switchClass;

    private SpriteRenderer spriteRenderer = null;

    // 컬러 파,노,파,노 로 바꾸는 코루틴 사용하려면 아래 주석 사용하면됨
    private Coroutine changeColor = default;
    // changeColor = StartCoroutine(ColorChange());

    private WaitForFixedUpdate waitForFixedUpdate = default;

    private Color32 blue = new Color32(40, 130, 220, 255);
    private Color32 yellow = new Color32(255, 180, 0, 255);


    //  원본 Sclae값
    private Vector3 defaultScale = default;
    // 줄여야하는 Sclae값
    private Vector3 shrinkageScale = default;

    //  현재sclae - shrinkageScale 하고 대입할 값
    private Vector3 reductionScale;

    private bool shotLaserIsbuttonOn = true;

    // Start is called before the first frame update
    public void Awake()
    {
        Initialization();
    }
    void Start()
    {
        //Debug.LogFormat("R : {0} / G:{1} / B : {2}", blue.r, blue.g, blue.b);
        //spriteRenderer.material.color = blue;
        Initialization();
        switchClass = FindObjectOfType<Switch>();
        switchClass.switchButtionboolChanged += ShotLaserControlerIsButtonOn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
            // SetActive == true 될 때 수행할 작업

        if (shotLaserIsbuttonOn == true)
        {
            Initialization();
            changeColor = StartCoroutine(ColorChange());
        }
    }

    void OnDisable()
    {
            // SetActive == flase 될 때 수행할 작업

        if (shotLaserIsbuttonOn == true)
        {
            // 코루틴에서 줄어든 크기 False될떄에 복구
            this.gameObject.transform.localScale = defaultScale;
            dottedLine.SetActive(true);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        //  레이저에 닿은것이 플레이어라면
        if (collision.gameObject.CompareTag("Player"))
        {
            // 참조해와야할 변수가 비어있으면 그떄만 가져옴
            if (player == default || player == null)
            {
                player = FindObjectOfType<SG_PlayerMovement>();
            }
            else { /*PASS*/ }

            player.isPlayerDie = true;

        }
    }

    // 초기화 해주는 함수
    public void Initialization()
    {
        // 스프라이트 랜더러가 비어있다면 GetComponent 안비어있으면 Pass
        if (spriteRenderer == default || spriteRenderer == null)
        {
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }
        else { /*PASS*/ }


        // { LEGACY : Color32 는 이런식 기입 불가
        //// 파란색 RGB가 들어가있지 않을때에만 색 RGB 기입
        //if (blue == default || blue == null)
        //{
        //    blue = new Color32(40, 130, 220,255);
        //}
        //else { /*PASS*/ }

        //// 노란색 RGB가 들어가 있지 않을때에만 색 RGB 기입
        //if (yellow == default || yellow == null)
        //{
        //    yellow = new Color32(255, 180, 0,255);
        //}
        //else { /*PASS*/ }
        // } LEGACY : Color32 는 이런식 기입 불가


        //  waitForFixedUpdate 가 비어있을때에 캐싱
        if (waitForFixedUpdate == default || waitForFixedUpdate == null)
        {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }
        else { /*PASS*/ }

        // 레이저 쏘면서 줄어들 값 여러번 사용하기에 캐싱
        if (shrinkageScale == default || shrinkageScale == null)
        {
            shrinkageScale = new Vector3(0.02f, 0f, 0f);
        }
        else { /*PASS*/ }

        // Scale X 를 줄이고 Active = false 할때에 원래 크기로 돌아오게할 변수
        if (defaultScale == default || defaultScale == null)
        {
            defaultScale = new Vector3(0.25f, 4.93f, 1f);
        }
    }

    private void ShotLaserControlerIsButtonOn(bool buttonSwitch)
    {
        shotLaserIsbuttonOn = buttonSwitch;
    }

    public void LaserAttack()
    {

    }

    IEnumerator ColorChange()
    {

        //  {파,노,파,노 로직
        spriteRenderer.color = blue;
        for (int i = 0; i <= 4; i++)
        {

            reductionScale = this.gameObject.transform.localScale -= shrinkageScale;
            this.gameObject.transform.localScale = reductionScale;
            yield return waitForFixedUpdate;
        }

        spriteRenderer.color = yellow;

        for (int j = 0; j <= 4; j++)
        {
            reductionScale = this.gameObject.transform.localScale -= shrinkageScale;
            this.gameObject.transform.localScale = reductionScale;
            yield return waitForFixedUpdate;
        }

        spriteRenderer.color = blue;

        for (int j = 0; j <= 4; j++)
        {
            reductionScale = this.gameObject.transform.localScale -= shrinkageScale;
            this.gameObject.transform.localScale = reductionScale;
            yield return waitForFixedUpdate;
        }
        //  }파,노,파,노 로직

        spriteRenderer.color = yellow;

        this.gameObject.SetActive(false);


    }
}
