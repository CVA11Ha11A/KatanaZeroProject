using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_Doctor : MonoBehaviour
{

    //플레이어의 위치 확인을위한 선언
    public Transform playerPotision;

    Animator animator;
    Rigidbody2D rigid;

    Coroutine coroutine;
    WaitForFixedUpdate waitForFixedUpdate;

    public GameObject doctorHeadPrefab;
    GameObject headClone;
    SpriteRenderer spriteRenderer;

    AudioSource audioSource;
    BoxCollider2D doctorCollider;
    /// <summary>
    /// [0] = 박사 힘들어하는소리 [1] = 목의 폭탄의 위험 소리 [2] = 목에 폭탄이 터지는 소리
    /// </summary>
    public AudioClip[] audioClip;

    // 플레이어와 만났는지 확인
    private bool meetPlayer = false;
    // 처음 의자 일어나는 컷신이 실행되었는지 확인
    private bool isStandCutin = false;
    //박사의 머리가 터질순간인지 확인할 변수
    private bool doctorBoomTime = false;
    // 머리가 터졌는지 확인하는 변수
    private bool doctorBoom = false;
    // 박사가 뛰고 있는지 체크할 변수
    private bool doctorRun = false;
    // 박사 머리를 Instance 했는지 확인할 변수
    private bool makeHead = false;

    // 박사 cutin 이 끝났는지 알려줄 변수 박사가 쓰러지고 2~3초뒤에 엔딩이 실행되게 하기위한 변수
    public bool isCutinEnding = false;

    private bool neckWarningSoundPlay = false;

    //플레이어가 내 x 좌표보다
    //뒤에 있으면 isBack = true
    //앞에 있으면 isFront = true
    private bool isFront = false;
    private bool isBack = false;

    Vector3 frontScale;
    Vector3 backScale;


    private float dis;
    private float moveSpeed = 4f;


    public void Awake()
    {
        doctorCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        waitForFixedUpdate = new WaitForFixedUpdate();

        audioSource.volume = 0.3f;
        frontScale = new Vector3(1.0f, 1.0f, 1.0f);
        backScale = new Vector3(-1.0f, 1.0f, 1.0f);

        this.gameObject.transform.localScale = backScale;
        animator.Play("Chair");

    }

    void Start()
    {

    }


    void Update()
    {
        dis = Vector3.Distance(playerPotision.position, this.transform.position);
        IsMeet();
        DoctorRun();
        WarningNeckSound();
        MakeHead();

    }


    // PlayCutin 001
    public void IsMeet()
    {
        if (meetPlayer == false && isStandCutin == false)
        {
            if (playerPotision.position.x > 26.3f && playerPotision.position.y > 9f)
            {
                meetPlayer = true;
                coroutine = StartCoroutine(PlayCutin002());
            }

        }
    }

    // PlayCutin 002
    IEnumerator PlayCutin002()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return waitForFixedUpdate;
        }
        animator.SetTrigger("ChairToStandUp");

        for (int i = 0; i < 100; i++)
        {
            yield return waitForFixedUpdate;
        }

        isStandCutin = true;
    }

    // PlayCutin 003
    // 플레이어와 일정거리를 유지하면서 멀어지면 뛰어서 따라가야함
    public void DoctorRun()
    {
        // 일어나는 컷신이 끝난뒤에 안에 조건에 들어갈수 있음
        if (meetPlayer == true && isStandCutin == true)
        {
            //플레이어가 나보다 앞에 있나 뒤에있나에 따라서 스케일값 조정해주는 if            
            if (this.transform.position.x > playerPotision.position.x && doctorBoomTime == false) //플레이어 X 포지션이 박사의 X 포지션 보다 작을떄
            {
                this.gameObject.transform.localScale = backScale;
                isBack = true;
                isFront = false;
            }
            else if (this.transform.position.x < playerPotision.position.x && doctorBoomTime == false) //플레이어 X 포지션이 박사의 X 포지션 보다 클때
            {
                this.gameObject.transform.localScale = frontScale;
                isFront = true;
                isBack = false;
            }

            // 플레이어가 닥터보다 X 좌표 작을때에 움직임
            if (isBack == true && dis > 1.5f && doctorBoomTime == false)
            {
                doctorRun = true;
                animator.SetBool("DoctorRun", doctorRun);

                Vector3 move = new Vector3(-moveSpeed, rigid.velocity.y, 0f);
                rigid.velocity = move;
                //rigid.velocity
            }
            else if (isBack == true && dis <= 1.5f && doctorBoomTime == false)
            {
                doctorRun = false;
                animator.SetBool("DoctorRun", doctorRun);

                rigid.velocity = Vector3.zero;
            }

            // 플레이어가 닥터보다 X 좌표 클떄에 움직임
            if (isFront == true && dis > 1.5f && doctorBoomTime == false)
            {
                doctorRun = true;
                animator.SetBool("DoctorRun", doctorRun);

                Vector3 move = new Vector3(moveSpeed, rigid.velocity.y, 0f);
                rigid.velocity = move;
                //rigid.velocity
            }
            else if (isFront == true && dis <= 1.5f && doctorBoomTime == false)
            {
                doctorRun = false;
                animator.SetBool("DoctorRun", doctorRun);

                rigid.velocity = Vector3.zero;
            }

            // 머리가 터질 시간인지 확인
            if (doctorBoomTime == false && this.transform.position.x <= 17f)
            {
                doctorBoomTime = true;
                coroutine = StartCoroutine(DoctorBoom());
            }
            else { /*PASS*/ }


        }
    }   //DoctorRun()

    // PlayCutin 004
    // 박사의 머리 터지게 만드는 코루틴
    IEnumerator DoctorBoom()
    {
        rigid.velocity = Vector3.zero;

        for (int i = 0; i <= 5; i++)
        {
            yield return waitForFixedUpdate;
        }
        audioSource.clip = audioClip[0];
        audioSource.Play();
        animator.Play("DoctorFall");

        doctorBoom = true;
        isFront = false;
        isBack = false;

        // 10 초뒤 엔딩 나오게 하기 위한 딜레이
        yield return new WaitForSeconds(3f);
        isCutinEnding = true;
    }

    //PlayCutin 005
    // sprite 가 머리 터지는 순간일때에 박사머리를 Instance하는함수
    public void MakeHead()
    {
        if(spriteRenderer.sprite.name == ("spr_factorysci_ground_head_explode_3") && makeHead == false)
        {
            audioSource.clip = audioClip[2];
            audioSource.Play();
            makeHead = true;
            headClone = Instantiate(doctorHeadPrefab, this.gameObject.transform.position, this.gameObject.transform.localRotation);

        }
    }

    //!Sound [1]의 클립은 어찌 넣으려면 코루틴 시간으로 쟤야하는데 프레임에 따라 다를수 있기에 박사의 스프라이트로 구별
    public void WarningNeckSound()
    {
        if(spriteRenderer.sprite.name == ("spr_factorysci_fall_17") && neckWarningSoundPlay == false)
        {
            neckWarningSoundPlay = true;
            audioSource.clip = audioClip[1];
            audioSource.Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, doctorCollider);
        }
    }

}   //NameSpace
