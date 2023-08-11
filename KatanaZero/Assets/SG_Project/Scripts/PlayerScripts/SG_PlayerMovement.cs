using Rewired;
using Rewired.ComponentControls.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
{
    // ============== 마우스 좌표를 위한 테스트 변수들=================
    //public GameObject testObj;
    //private GameObject cloneObj;

    // 아래는 임펙트조건을 위한 int형 변수
    public int one = 0;


    // ============== 마우스 좌표를 위한 테스트 변수들=================

    public float moveSpeed = 10f;
    public float jumpForce = 7f;



    #region Bool 변수

    // TEST BOOL
    private bool PresentWallGrab = false;
    // TEST BOOL

    // 플레이어가 죽었는지 살았는지 구별할 변수 다른곳에서 참조를위해 public 으로 열어둠
    public bool isPlayerDie = false;

    private bool isJump = false;



    //나중에 플레이어 공중처리할 Bool변수
    public bool playerIsAir = false;
    public bool isAttacking = false;
    public bool readyRun = false;
    public bool leftClickAttackCoolTime = false;

    // 구르기 제작에 사용할 변수들
    public bool isRightMove = false;
    public bool isLeftMove = false;
    public bool isDownKeyInput = false;
    public bool isRollRock = false;
    public bool isRolling = false;


    // 벽붙기 제작에 사용할 변수들
    public bool isWallGrab = false;

    // 붙을수 있는 벽에 붙거나 나갈때에 변경될 변수
    private bool exitWallGrab = false;


    // 벽에 붙은뒤 대각선 점프할때 필요한 변수
    private bool wallJump = false;
    public bool isleftWall = false;
    public bool isRightWall = false;

    // 벽에 붙은뒤 공격시 에러가 나기때문에 만든 변수
    private bool isAttackClingWallCoolTimeBool = false;


    // 덤블링후 앉지 못하게 처리할 변수
    //  true상태일때 못하게 할거임
    private bool headOffPrecrouch = false;


    // 플레이어 데미지 받을수 있는 상태인지 체크하는 변수
    public bool isDodge = false;

    // 현재플레이어가Floor 에  땅에 닿아 있는지 알려줄 Bool변수
    private bool playerPresentFloor = false;

    #endregion

    #region int 변수

    //플레이어가 3번이상 연속으로 공격하지 못하게 할 변수 (collider에서 적이 아닐경우 초기화 되게 해두었음)
    public int attackCount = 0;

    // 애니메이션 과도한 연속재생 방지 변수
    public int wallGrabCount = 0;

    #endregion

    public Vector3 mousePosition;

    Player player;
    Rigidbody2D playerRigid;
    Animator playerAni;
    BoxCollider2D playerBoxCollider;
    public GameObject leftAttackObj;

    Vector3 frontScale = Vector3.one;
    Vector3 backScale = new Vector3(-1f, 1f, 1f);

    // ================= 코루틴 ======================

    private Coroutine leftAttackCoroutine;    // 코루틴 캐싱
    private Coroutine moveRock;
    private Coroutine addforceReset;
    private Coroutine rollRock;
    private Coroutine isAttackClingWallCoolTime;


    // 아래 fixedUpdate는 0.02초 주기이기에 계산 잘해야할거같음
    public WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate(); // yield return fixedUpdate캐싱
    public WaitForSeconds waitSecond = new WaitForSeconds(0.3f);


    // ================= 코루틴 ======================


    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        playerRigid = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();

        leftAttackObj.SetActive(false);


    }
    public void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {

        LeftMove();
        RightMove();
        IsDownKey();
        IsRoll();
        JumpMove();
        WallGrabAtInput();
        AttackClick();

        if(isPlayerDie == true)
        {
            Debug.Log("플레이어 Die == true");
        }


    }

    //========================================콜라이더=======================================

    #region        콜라이더



    // --------------------------------------Collision Enter------------------------------------
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // 아래 변수를 조건을 조금 변경해야 함 SG_ClingWall이라는 벽이 아니라면 으로 조건걸고 넣어야함
        headOffPrecrouch = false;

        // ================================태그가 적일때 ========================================
        #region
        if (collision.gameObject.CompareTag("Enemy"))
        {

        }
        else
        {
            attackCount = 0;
            isJump = false;
        }
        #endregion

        //Debug.LogFormat("Enter -> {0}", collision.gameObject.tag);



        ////여기서 다시 점프가능
        //isJump = false;
        //playerAni.SetBool("IsJumpBool", isJump);

        //아래변수는 플레이어가 공중에 있나? 를 의미
        // 애니메이션 컨트롤러 수정 이후 다시 확인할예정
        playerIsAir = false;



        // ================================= 태그가 붙을수 있는 벽일때 SG_ClingWall =================================
        #region
        // 아래는 붙을수 있는 벽에 붙었을때에 if (Collider)
        //if (collision.gameObject.CompareTag("SG_ClingWall"))
        //{
        //    Debug.LogFormat("조건 1: {0} / 조건 2: {1}", isAttackClingWallCoolTimeBool, playerPresentFloor);
        //}

        if (collision.gameObject.CompareTag("SG_ClingWall") && isAttackClingWallCoolTimeBool == false &&
            playerPresentFloor == false)
        {
            //Debug.Log("Enter에서 붙는벽 인식");

            
            playerAni.SetTrigger("WallGrabTrigger");
            //Debug.LogFormat("Trigger실행시킴");

            // 점프해서 벽에 붙으면 달리지 않는다.
            readyRun = false;
            //playerAni.SetBool("ReadyRun", readyRun);



            //// TEST
            //PresentWallGrab = true;
            //playerAni.SetBool("PresentWallGrab", PresentWallGrab);
            //// TEST


            exitWallGrab = false;
            isWallGrab = true;
            wallGrabCount = 1;

            // 여기다 bool 형 오른쪽 왼쪽 넘겨주어야함
            if (collision.gameObject.transform.position.x < this.gameObject.transform.position.x)
            {
                isleftWall = true;
            }
            else if (collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                isRightWall = true;
            }
            else { /*PASS*/ }

            // 아래는 붙는 벽에따라 플레이어 좌,우 변경 if문
            if (isleftWall == true)
            {
                this.gameObject.transform.localScale = backScale;
            }
            else if (isRightWall == true)
            {
                this.gameObject.transform.localScale = frontScale;
            }
            else { /*PASS*/ }

            //Debug.Log("CollisionEnter 어느 주기로 체크?");
            if (playerRigid.gravityScale == 1 && playerRigid.mass > 0)
            {

                playerRigid.gravityScale = 0.3f;
                playerRigid.mass = 0.3f;

            }
            else { /*PASS*/ }

            //playerAni.SetTrigger("WallGrabTrigger");

            //Debug.LogFormat("벽에 붙에 붙었는지? PresentWallGrab: {0}, isWallGrab: {1}, Jump: {2}, isRun: {3}", 
            //    PresentWallGrab, isWallGrab, playerAni.GetBool("IsJump"), playerAni.GetBool("ReadyRun"));

            //if (wallGrabCount == 1)
            //{
            //playerAni.SetTrigger("WallGrabTrigger");

            //}

        }


        #endregion
        // 덤블링 후 바닥에 닿았을떄에 if

        // ================================= 태그가 바닥일때 Floor ================================================
        #region

        if (collision.gameObject.CompareTag("Floor"))
        {
            //여기서 다시 점프가능
            isJump = false;

            playerPresentFloor = true;

            //// TEST
            //PresentWallGrab = false;
            //playerAni.SetBool("PresentWallGrab", PresentWallGrab);
            //// TEST


            playerAni.SetBool("IsJumpBool", isJump);

            playerAni.SetTrigger("GrabwallToIdleTrigger");

        }


        if (collision.gameObject.CompareTag("Floor") && wallJump == true)
        {
            wallJump = false;
            playerAni.SetBool("WallJumpBool", wallJump);

        }

        if(collision.gameObject.CompareTag("Floor") && isWallGrab == true)
        {
            
        }
        #endregion
        // !플레이어가 무적이 아닐때
        if (isDodge == false)
        {


        }
        // !플레이어가 무적이 아닐때



    }   //OnCollisionEnter


    //------------------------------------------- Collision Stay ---------------------------------------------
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SG_ClingWall"))
        {
            //Debug.Log("CollisionStay 어느 주기로 체크?");

            // 천천히 내려오게 하기 위해 중력 0으로 설정

            this.gameObject.transform.position = this.gameObject.transform.position - new Vector3(0f, 0.001f, 0f);

        }

        //Debug.LogFormat("Stay -> {0}", collision.gameObject.tag);

    }   // OnCollisionStay

    //------------------------------------------ Collision Exit --------------------------------------------
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (playerRigid.gravityScale == 0)
        {
            playerRigid.mass = 1;
            playerRigid.gravityScale = 1;
        }
        else { /*PASS*/ }

        if (collision.gameObject.CompareTag("SG_ClingWall"))
        {
            exitWallGrab = true;

            //// TEST
            //PresentWallGrab = false;
            //playerAni.SetBool("PresentWallGrab", PresentWallGrab);
            //// TEST

        }
        else { /*PASS*/ }

        if(collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("Exit에서 False로 바꿈");
            playerPresentFloor = false;
        }

        //Debug.LogFormat("Exit -> {0}", collision.gameObject.tag);
        

        ////  바닥에선 점프가 되진 않음 하지만 다시 벽에 붙었을때 점프를 위해 일단 놔둠
        //if (collision.gameObject.CompareTag("SG_ClingWall"))
        //{
        //    isJump = false;
        //}


        //  나중에 이상한곳에서 false가 된다면 위에 mass,gravityScale 1로 만드는 조건문 안에 넣으면 될거같음
        isleftWall = false;
        isRightWall = false;


    }

    #endregion 콜라이더


    //=======================================커스텀 함수=========================================


    #region 이동관련 커스텀함수
    public void LeftMove()
    {

        if (isAttacking == false && isRolling == false)
        {
            //if (player.GetButtonDown("MoveLeft") && isDownKeyInput == false)
            //{
            //    playerAni.SetTrigger("ReadyRunTrigger");
            //}

            if (player.GetButton("MoveLeft"))
            {
                isLeftMove = true;
                // Debug.LogFormat("L이동 -> {0}", isLeftMove);
                //gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                this.gameObject.transform.localScale = backScale;
                readyRun = true;
                Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;
                playerAni.SetBool("ReadyRun", readyRun);

            }

            if (player.GetButtonUp("MoveLeft"))
            {
                isLeftMove = false;
                //Debug.LogFormat("L이동 -> {0}", isLeftMove);
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);
            }

        }


    }

    public void RightMove()
    {

        if (isAttacking == false && isRolling == false)
        {

            //if (player.GetButtonDown("MoveRight") && isDownKeyInput == false)
            //{
            //    playerAni.SetTrigger("ReadyRunTrigger");
            //}

            if (player.GetButton("MoveRight"))
            {
                //Debug.LogFormat("R이동 -> {0}", isRightMove);
                isRightMove = true;
                //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                this.gameObject.transform.localScale = frontScale;
                readyRun = true;
                Vector3 move = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;

                playerAni.SetBool("ReadyRun", readyRun);

            }
            if (player.GetButtonUp("MoveRight"))
            {
                isRightMove = false;
                //Debug.LogFormat("R이동 -> {0}", isRightMove);
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);
            }
        }



    }
    public void IsDownKey()
    {

        if (player.GetButtonDown("DownKey") && headOffPrecrouch == false)
        {
            isDownKeyInput = true;
            playerAni.SetBool("PrecrouchBool", isDownKeyInput);
            //Debug.LogFormat("S키 -> {0}", isDownKeyInput);

            playerBoxCollider.offset = new Vector2(0.07f, -0.27f);
            playerBoxCollider.size = new Vector2(0.55f, 0.64f);

        }
        else { /*PASS*/ }

        if (player.GetButton("DownKey") && headOffPrecrouch == false)
        {
            isDownKeyInput = true;
        }

        if (player.GetButtonUp("DownKey") && headOffPrecrouch == false)
        {
            isDownKeyInput = false;
            playerAni.SetBool("PrecrouchBool", isDownKeyInput);
            //Debug.LogFormat("S키 -> {0}", isDownKeyInput);

            playerBoxCollider.offset = new Vector2(0.07f, -0.03f);
            playerBoxCollider.size = new Vector2(0.55f, 1.1f);
        }
        else { /*PASS*/ }
    }

    public void IsRoll()
    {
        if (isRollRock == false && playerIsAir == false && isJump == false)
        {
            if (isRightMove == true && isDownKeyInput == true
                && isLeftMove == false && isRollRock == false)
            {
                isDodge = true;
                //Debug.Log("구르기 들어옴");
                //

                playerAni.SetTrigger("Let's_Roll");
                rollRock = StartCoroutine(RollCoolTime());
                playerAni.SetBool("IsRollRock", isRollRock);
                // 달리기 끄는 로직
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);

                Debug.LogFormat("구르기 끝났을 때 / isRollRock: {0}, readyRun: {1}",
                    isRollRock, readyRun);

            }       // if: 오른쪽 구르기인지?
            else { /*PASS*/ }

            if (isLeftMove == true && isDownKeyInput == true &&
                isRightMove == false && isRollRock == false)
            {
                isDodge = true;

                playerAni.SetTrigger("Let's_Roll");
                rollRock = StartCoroutine(RollCoolTime());
                playerAni.SetBool("IsRollRock", isRollRock);
                // 달리기 끄는 로직
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);

                //Debug.LogFormat("구르기 끝났을 때 / isRollRock: {0}, readyRun: {1}",
                    //isRollRock, readyRun);
            }       // if: 왼쪽 구르기인지?
            else { /*PASS*/ }


        }

        //else { Debug.Log("구르기 잠김"); } 
    }

    public void JumpMove()
    {
        //Debug.LogFormat("현재 점프 상태?? Ani jump: {0}, current jump: {1}", 
        //playerAni.GetBool("IsJumpBool"), isJump);

        if (player.GetButtonDown("MoveJump") && isJump == false && isWallGrab == false)
        {
            if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
            {
                playerRigid.mass = 1;
                playerRigid.gravityScale = 1;
            }

            playerIsAir = true;

            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            //점프 했을때에 또 점프 못하게 bool 처리
            playerAni.SetTrigger("IsJump");

            isJump = true;
            playerAni.SetBool("IsJumpBool", isJump);
            headOffPrecrouch = true;

        }

        //  붙는 벽에 달라붙은 상태에서 점프를 했을경우
        else if (player.GetButtonDown("MoveJump") && isJump == false && isWallGrab == true)
        {
            if (isleftWall == true)
            {
                //  점프전에 중력 정상화
                if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
                {
                    playerRigid.mass = 1;
                    playerRigid.gravityScale = 1;
                }
                wallJump = true;
                playerAni.SetBool("WallJumpBool", wallJump);

                this.gameObject.transform.localScale = backScale;
                playerRigid.AddForce(new Vector2(8f, 9f), ForceMode2D.Impulse);

                playerAni.SetTrigger("PlayerFlipTrigger");
                this.gameObject.transform.localScale = frontScale;
                playerAni.SetTrigger("FlipAfterTrigger");

                isJump = true;
                isWallGrab = false;
                headOffPrecrouch = true;
            }

            else if (isRightWall == true)
            {
                //  점프전에 중력 정상화
                if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
                {
                    playerRigid.mass = 1;
                    playerRigid.gravityScale = 1;
                }
                wallJump = true;
                playerAni.SetBool("WallJumpBool", wallJump);

                this.gameObject.transform.localScale = frontScale;
                playerRigid.AddForce(new Vector2(-8f, 9f), ForceMode2D.Impulse);

                playerAni.SetTrigger("PlayerFlipTrigger");
                this.gameObject.transform.localScale = backScale;
                playerAni.SetTrigger("FlipAfterTrigger");

                isJump = true;
                isWallGrab = false;
                headOffPrecrouch = true;



            }
            //  TODO : 콜라이더만이 지금 붙어 있는 아이의 좌표를 알수 있으니 뒤에 Bool 형 변수 LEFT,Right 만들어서 여기로 넘겨주고
            //         오른쪽일때 주는 힘 왼쪽일떄 주는힘 다르게 해야 함
        }
    }

    // Wall Grab상태일때에 키를 눌른다면
    public void WallGrabAtInput()
    {
        //if (isWallGrab == true && !player.GetButtonDown("MoveJump") && exitWallGrab == true)
        //{
           
        //    isWallGrab = false;
        //    //exitWallGrab = false;
        //}
        //else { /*PASS*/ }


        //  { if : 벽에붙어있으며 W키가아닌 A,S,D 키중 하나라도 눌렀을경우 
        if(isWallGrab == true && 
            (player.GetButtonDown("MoveLeft") || player.GetButtonDown("MoveRight") || player.GetButtonDown("DownKey")))
        {
            // 쿨타임을 주어서 벽에서 붙은판정으로 가지 못하게 막음
            exitWallGrab = true;
            playerAni.SetTrigger("WallGrabToFallTriger");
            StartCoroutine(IsAttackClingWallCoolTime());
            //exitWallGrab = false;

        }   //  } if : 벽에붙어있으며 W키가아닌 A,S,D 키중 하나라도 눌렀을경우 

    }



    #endregion

    public void AttackClick()
    {


        //  좌클릭시
        if (Input.GetMouseButtonDown(0))
        {


            if (leftClickAttackCoolTime == false && attackCount < 3) // 좌클릭 쿨타임 아닐때에 실행됨                        
            {
                //Debug.Log("!공격");
                isleftWall = true;

                if (isWallGrab == true)
                {
                    isAttackClingWallCoolTimeBool = true;
                    isAttackClingWallCoolTime = StartCoroutine(IsAttackClingWallCoolTime());
                }
                else { /*PASS*/ }

                // 플레이어 중력 정상화 로직
                if (playerRigid.gravityScale < 1)
                {
                    playerRigid.mass = 1;
                    playerRigid.gravityScale = 1;
                }
                else { /*PASS*/ }

                if (isRightWall == true || isleftWall == true)
                {
                    isleftWall = false;
                    isRightWall = false;
                }
                else { /*PASS*/ }

                leftAttackObj.SetActive(true);
                one = 1;
                attackCount += 1;
                readyRun = false;
                isAttacking = true;
                leftAttackCoroutine = StartCoroutine(LeftClickAttack());
                moveRock = StartCoroutine(MoveRock());

                playerAni.SetTrigger("LeftClickAttack");
                //Debug.LogFormat("마우스왼쪽클릭했음");


                //  마우스의 좌표를 카메라의 WorldPoint로 구함 
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                   Input.mousePosition.y, -Camera.main.transform.position.z));

                #region 좌표 REGACY
                //아래 Instantiate 는 마우스찍는곳이 정상인지 확인하기 위한 박스 제작기
                //cloneObj = Instantiate(testObj, mousePosition, testObj.transform.rotation);



                //  아래 조건 사용시 좌우로 움직이질 않기 때문에 잠시 주석
                // 플레이어 y포지션 + 0.2 하면 어꺠 쪽
                //if (this.gameObject.transform.position.y + 0.2 < mousePosition.y && 
                //    this.gameObject.transform.position.x + 0.2 > mousePosition.x ||
                //    this.gameObject.transform.position.x + -0.01 > mousePosition.x)
                //{
                //    // 윗 공격 조건 달성시 좌우 이동없이 위로 이동
                //    //  PASS
                //}
                #endregion 좌표 REGACY

                //  ==================== 아래는 조건에따라 AddForce로 힘을 주는 코드 =========================
                //  클릭시 X좌표 조건
                if (mousePosition.x > gameObject.transform.position.x)
                {
                    // 계속 눌를시 AddForce 가속화되기때문에 추가전 초기화
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(12f, 0f), ForceMode2D.Impulse);
                    //힘을 실어준다음에 시간지나면 초기화시켰고 상,하 는 어차피 좌우도 같이 움직일수 밖에 없기 때문에 삽입X
                    addforceReset = StartCoroutine(AddforceReset());
                    this.gameObject.transform.localScale = frontScale;
                }
                else if (mousePosition.x < gameObject.transform.position.x)
                {
                    // 계속 눌를시 AddForce 가속화되기때문에 추가전 초기화
                    playerRigid.velocity = Vector3.zero;

                    playerRigid.AddForce(new Vector2(-12f, 0f), ForceMode2D.Impulse);
                    //힘을 실어준다음에 시간지나면 초기화시켰고 상,하 는 어차피 좌우도 같이 움직일수 밖에 없기 때문에 삽입X
                    addforceReset = StartCoroutine(AddforceReset());

                    this.gameObject.transform.localScale = backScale;
                }
                else { /*PASS*/ }

                // 클릭시 Y좌표 조건
                if (mousePosition.y > gameObject.transform.position.y)
                {
                    playerAni.SetBool("IsJumpBool", true);
                    playerIsAir = true;
                    playerRigid.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
                }
                else if (mousePosition.y < gameObject.transform.position.y)
                {
                    playerAni.SetBool("IsJumpBool", true);
                    playerIsAir = true;
                    playerRigid.AddForce(new Vector2(0f, -5f), ForceMode2D.Impulse);
                }
                else { /*PASS*/ }

                //Debug.LogFormat("Now AirBool -> {0}", playerIsAir);
                playerAni.SetBool("PlayerIsAir", playerIsAir);

                // 특정한 시점에서 공격후 달리기 떄문에 달리기 끄는 로직 추가
                // 달리기 끄는 로직
                readyRun = false;
                playerAni.SetBool("ReadyRun", readyRun);

            }   // 좌클릭 쿨타임 아닐때에 실행됨

            else { /*PASS*/ }
            //{ /*Debug.LogFormat("true = 쿨타임이 돌고있음 ->{0}", leftClickAttackCoolTime);*/ }

            //Debug.LogFormat("이것이 현재 마우스의 좌표? -> {0}", mousePosition);
        }

    }   // AttackClick()




    // ==========================================코루틴============================================

    // { 왼쪽 공격 쿨타임 코루틴
    private IEnumerator LeftClickAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return fixedUpdate;
        }


        leftClickAttackCoolTime = true;

        yield return waitSecond;    // waitSecond = 0.3초
        leftAttackObj.SetActive(false);
        leftClickAttackCoolTime = false;

    }
    // } 왼쪽 공격 쿨타임 코루틴


    // { 공격시 일정시간 움직임 잠금
    private IEnumerator MoveRock()
    {
        for (int i = 0; i <= 25; i++)
        {
            //Debug.LogFormat("현재 fixedUpdate 돈 횟수 {0}", i);
            // 1초동안 움직임 멈춤
            yield return fixedUpdate;
        }

        isAttacking = false;
    }
    // } 공격시 일정시간 움직임 잠금

    // { 공격후 AddForce의 힘값을 일정시간 이후 초기화
    private IEnumerator AddforceReset()
    {
        for (int i = 0; i <= 15; i++)
        {
            yield return fixedUpdate;
        }

        //  플레이어보다 앞을 눌렀을때에
        if (mousePosition.x > gameObject.transform.position.x)
        {
            playerRigid.velocity = Vector3.one;
        }
        else if (mousePosition.x < gameObject.transform.position.x)
        {
            playerRigid.velocity = Vector3.one * -1;
        }
    }
    // } 공격후 AddForce의 힘값을 일정시간 이후 초기화


    // { 구르기 한뒤에 쿨타임을 줄예정
    private IEnumerator RollCoolTime()
    {

        isRollRock = true;
        isRolling = true;
        // 여기서 구를 때에 앞으로 힘을 가해주면 될거같은데?

        if (isRightMove == true && playerIsAir == false && isJump == false) // 오른쪽 구르기
        {
            playerRigid.AddForce(new Vector3(8f, 0f, 0f), ForceMode2D.Impulse);
            for (int i = 0; i <= 10; i++)
            {
                yield return fixedUpdate;
            }
            playerRigid.velocity = Vector3.zero;
            isRolling = false;


            for (int j = 0; j <= 17; j++)
            {
                yield return fixedUpdate;
            }
            isRightMove = false;
            isDodge = false;
        }   // 오른쪽 구르기

        if (isLeftMove == true && playerIsAir == false && isJump == false) // 왼쪽 구르기
        {
            playerRigid.AddForce(new Vector3(-8f, 0f, 0f), ForceMode2D.Impulse);
            for (int i = 0; i <= 10; i++)
            {
                yield return fixedUpdate;
            }
            playerRigid.velocity = Vector3.zero;
            isRolling = false;


            for (int j = 0; j <= 17; j++)
            {
                yield return fixedUpdate;
            }
            isLeftMove = false;
            isDodge = false;
        }   // 왼쪽 구르기
        isRollRock = false;


        //Debug.LogFormat("readyRun -> {0}", readyRun);
        //Debug.Log("구르기쿨 끝남");
    }
    // } 구르기 한뒤에 쿨타임을 줄예정

    private IEnumerator IsAttackClingWallCoolTime()
    {

        yield return waitSecond;
        isAttackClingWallCoolTimeBool = false;
        exitWallGrab = false;
    }



}   // NameSpace
