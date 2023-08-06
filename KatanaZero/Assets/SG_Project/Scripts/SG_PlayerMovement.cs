using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 7f;


    private bool isJump = false;
    private bool playerIsAir = false;
    private bool readyRun = false;
    private bool leftClickAttackCoolTime = false;
    private Vector3 mousePosition;

    Player player;
    Rigidbody2D playerRigid;
    Animator playerAni;

    // ================= 코루틴 ======================

    private Coroutine leftAttackCoroutine;    // 코루틴 캐싱

    // 아래 fixedUpdate는 0.02초 주기이기에 계산 잘해야할거같음
    private WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate(); // yield return fixedUpdate캐싱
    private WaitForSeconds waitSecond = new WaitForSeconds(0.3f);
    

    // ================= 코루틴 ======================


    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        playerRigid = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();

        
    }

    // Update is called once per frame
    void Update()
    {
 
        LeftMove();
        RightMove();
        JumpMove();
        AttackClick();


    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //여기서 다시 점프가능
        isJump = false;
        //아래변수는 플레이어가 공중에 있나? 를 의미
        // 애니메이션 컨트롤러 수정 이후 다시 확인할예정
        //playerIsAir = false;
        playerAni.SetBool("IsJumpBool", isJump);

        //Debug.LogFormat("어딘가에 부딫쳤다. Jump: {0}", isJump);
    }


    //=======================================커스텀 함수=========================================
    public void LeftMove()
    {
        if (player.GetButton("MoveLeft"))
        {
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            readyRun = true;
            Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
            playerRigid.velocity = move;
            playerAni.SetBool("ReadyRun", readyRun);

        }

        if (player.GetButtonUp("MoveLeft"))
        {
            readyRun = false;
            playerAni.SetBool("ReadyRun", readyRun);
        }
    }

    public void RightMove()
    {
        if (player.GetButton("MoveRight"))
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            readyRun = true;
            Vector3 move = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
            playerRigid.velocity = move;
            playerAni.SetBool("ReadyRun", readyRun);

        }
        if (player.GetButtonUp("MoveRight"))
        {
            readyRun = false;
            playerAni.SetBool("ReadyRun", readyRun);
        }
    }

    public void JumpMove()
    {
        //Debug.LogFormat("현재 점프 상태?? Ani jump: {0}, current jump: {1}", 
            //playerAni.GetBool("IsJumpBool"), isJump);

        if (player.GetButtonDown("MoveJump") && isJump == false)
        {

        

            playerRigid.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse);

            //점프 했을때에 또 점프 못하게 bool 처리
            playerAni.SetTrigger("IsJump");

            isJump = true;
            playerAni.SetBool("IsJumpBool", isJump);

        }
    }

    public void AttackClick()
    {

        
        //  좌클릭시
        if (Input.GetMouseButtonDown(0))
        {

            if (leftClickAttackCoolTime == false) // 좌클릭 쿨타임 아닐때에 실행됨
            {
                
                leftAttackCoroutine = StartCoroutine(LeftClickAttack());

                playerAni.SetTrigger("LeftClickAttack");
                Debug.LogFormat("마우스왼쪽클릭했음");


                //  마우스의 좌표를 카메라의 WorldPoint로 구함 
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                   Input.mousePosition.y, -Camera.main.transform.position.z));

                //  클릭시 X좌표 조건
                if (mousePosition.x > gameObject.transform.position.x)
                {
                    // 계속 눌를시 AddForce 가속화되기때문에 추가전 초기화
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(9f, 0f), ForceMode2D.Impulse);
                    this.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (mousePosition.x < gameObject.transform.position.x)
                {
                    // 계속 눌를시 AddForce 가속화되기때문에 추가전 초기화
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(-9f, 0f), ForceMode2D.Impulse);
                    this.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else { /*PASS*/ }

                // 클릭시 Y좌표 조건
                if (mousePosition.y > gameObject.transform.position.y)
                {
                  
                    playerRigid.AddForce(new Vector2(0f, 3f), ForceMode2D.Impulse);
                }
                else if (mousePosition.y < gameObject.transform.position.y)
                {
                 
                    playerRigid.AddForce(new Vector2(0f, -3f), ForceMode2D.Impulse);
                }
                else { /*PASS*/ }

            }   // 좌클릭 쿨타임 아닐때에 실행됨

            else { Debug.LogFormat("true = 쿨타임이 돌고있음 ->{0}", leftClickAttackCoolTime); }

            //Debug.LogFormat("이것이 현재 마우스의 좌표? -> {0}", mousePosition);
        }
        
    }   // AttackClick()


    // ==========================================코루틴============================================

    // { 왼쪽 공격 쿨타임 코루틴
    private IEnumerator LeftClickAttack()
    {
        leftClickAttackCoolTime = true;

        yield return waitSecond;    // waitSecond = 0.3초

        leftClickAttackCoolTime = false;
    }
    // } 왼쪽 공격 쿨타임 코루틴


}   // NameSpace
