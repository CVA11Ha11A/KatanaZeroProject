using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TutorialPlayerFirstControlScript : MonoBehaviour
{
    public SG_PlayerMovement movement;
   
    public GameObject leftClickAttack;
    public GameObject playerLight;
    public GameObject enemyLight;
    public GameObject globalLight;
    public GameObject cmVCAM;
    
    public CinemachineVirtualCamera virtualCamera;

    public RuntimeAnimatorController newController;

    private Animator animator;

    private Coroutine playerStart;   

    // waitForSecond = 0.25초
    private WaitForSeconds waitForSecond;

    private WaitForFixedUpdate waitForFixedUpdate;

    private Rigidbody2D rigid;

    private StateMachineBehaviour stateMachine;
    


    int jumpState = 0;


    private bool isFall = false;

    // Idle = 0     Fall = 1     Roll = 2      Run = 3

    private float nowAni = 0;

    // 뛰어내려서 굴러야 할때 = 1 
    private float nowCutin = 0;

    private float moveSpeed = 9f;

    private bool userGameStart = false;
    private bool giveScripts = false;


    // Start is called before the first frame update
    public void Awake()
    {
        //stateMachine = GetComponent<StateMachineBehaviour>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        stateMachine = animator.GetBehaviour<AnimatorPlayerLayerScript>();

        waitForSecond = new WaitForSeconds(0.25f);
        waitForFixedUpdate = new WaitForFixedUpdate();

        movement.enabled = false;

    }

    void Start()
    {
        playerStart = StartCoroutine(StartTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        // 실행순서 4
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 0)
        {

            //Debug.Log("TimeSclae 이 0 이여도 되나?");
            animator.Play("Attack");
            leftClickAttack.SetActive(true);
            Time.timeScale = 1;
            playerLight.SetActive(false);
            enemyLight.SetActive(false);
            globalLight.SetActive(false);
            //cmVCAM.SetActive(false);  //좀 더 진행하고 해야함

            playerStart = null;
            playerStart = StartCoroutine(TimeEventAfterJump001());

        }

        // 튜토리얼 부분이 끝날때를 체크하는 함수
        GAMESTART();


    }

    //  실행순서 2
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") && nowAni == 1)
        {

            //isFall = false;
            animator.SetTrigger("ExitFall");
            animator.SetBool("FallBool", false);

            nowAni = 0;
            nowCutin = 1;

            rigid.velocity = Vector3.zero;
            this.rigid.AddForce(new Vector2(8f, 0f), ForceMode2D.Impulse);
            animator.Play("IsRoll");

            
            playerStart = null;
            playerStart = StartCoroutine(Play3());
            //animator.Play("Run");
        }
        else { /*PASS*/ }

        //  적 잡고 뒤돌아서 첫 점프후 바닥에 닿을때
        if (collision.gameObject.CompareTag("Floor") && jumpState == 1)
        {
            animator.SetTrigger("CreepUpTrigger");
            playerStart = null;
            rigid.gravityScale = 1f;
            playerStart = StartCoroutine(TimeEventAfterJump002());

        }
        else { /*PASS*/ }

        // 위 점프이후 코루틴 으로 점프하고 바닥에 닿았을떄
        if (collision.gameObject.CompareTag("Floor") && jumpState == 2)
        {
            animator.SetTrigger("BendDownTrigger");
            jumpState = 0;
            userGameStart = true;

            rigid.gravityScale = 1f;

            //this.transform.localScale = Vector3.one;
        }

    }

    //  실행순서 1
    private IEnumerator StartTutorial()
    {

        for (int i = 0; i <= 20; i++)
        {
            yield return waitForSecond;
        }

        this.rigid.AddForce(new Vector2(3f, 4f), ForceMode2D.Impulse);
        isFall = true;


        animator.SetBool("FallBool", isFall);
        //animator.Play("Fall");
        nowAni = 1;


    }

    // 실행순서3
    private IEnumerator Play3()
    {
        //while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
        //{
        //    yield return waitForFixedUpdate;
        //}

        //while(stateInfo.IsName("IsRool") && playbackTime < 0.8f)
        //{
        //    yield return waitForFixedUpdate;
        //}
        rigid.velocity = Vector3.zero;
        yield return waitForFixedUpdate;

        for (int i = 0; i <= 55; i++)
        {

            //animator.Play("run");
            Vector3 move = new Vector3(moveSpeed, rigid.velocity.y, 0f);
            rigid.velocity = move;
            yield return waitForFixedUpdate;


        }
        rigid.velocity = Vector3.zero;
        this.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);



    }

    private IEnumerator TimeEventAfterJump001()
    {
        for (int j = 0; j <= 4; j++)
        {
            yield return waitForSecond;
        }

        this.transform.localScale = Vector3.one;
        virtualCamera.Follow = this.gameObject.transform;

        for (int i = 0; i <= 3; i++)
        {
            yield return waitForSecond;
        }

        cmVCAM.transform.position = new Vector3(-15.5f, cmVCAM.transform.position.y, cmVCAM.transform.position.z);

        for (int k = 0; k <= 3; k++)
        {
            yield return waitForSecond;
        }

        rigid.AddForce(new Vector2(6f, 7f), ForceMode2D.Impulse);
        animator.Play("Fall002");
        jumpState = 1;
        for(int i = 0; i <= 2; i++)
        {
            yield return waitForSecond;
        }
        rigid.gravityScale = 1.5f;

    }
    private IEnumerator TimeEventAfterJump002()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return waitForSecond;
        }
        this.transform.localScale = new Vector3(-1f, 1f, 1f);

        yield return waitForFixedUpdate;

        rigid.AddForce(new Vector2(-4f, 5f), ForceMode2D.Impulse);

        animator.Play("Fall002");

        jumpState = 2;

        for(int i = 0; i <= 2; i++)
        {
            yield return waitForSecond;
        }

        rigid.gravityScale = 1.5f;


    }

    private void GAMESTART()
    {
        if (userGameStart == true && giveScripts == false)
        {
            Debug.Log("이제 돌려주어도 되나?");
            // 애니메이터와 스크립트 돌려줘야함
            ChangeController(newController);
            movement.enabled = true;

            // 스크립트와 애니메이터 돌려주고 연속적으로 주지 않기위한 변수
            giveScripts = true;

            enabled = false;

        }
    }

    private void ChangeController(RuntimeAnimatorController controller)
    {
        animator.runtimeAnimatorController = controller;
    }

}   // NAMESPACE
