using Rewired;
using Rewired.ComponentControls.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
{

    // 아래는 임펙트조건을 위한 int형 변수
    // one 변수는 AttackEmpect 스크립트에서 참조 하기 때문에 public
    public int one = 0;
    public bool dodgeReturn=false;

    // ============== 마우스 좌표를 위한 테스트 변수들=================

    public float moveSpeed = 10f;
    public float jumpForce = 7f;


    public GameObject gameOverUi;
    #region Bool 변수


    // private
    private bool isJump = false;

    SG_Ghost ghost;

    //나중에 플레이어 공중처리할 Bool변수

    public bool isAttacking = false;

    //private
    private bool readyRun = false;
    private bool leftClickAttackCoolTime = false;

    //private
    // 구르기 제작에 사용할 변수들
    private bool isRightMove = false;
    private bool isLeftMove = false;
    private bool isDownInput = false;
    private bool isRollRock = false;
    private bool isRolling = false;


    //private
    // 벽붙기 제작에 사용할 변수들
    // 23.08.18 이걸로 벽에 붙었는지 안붙었는지 체크 할거임
    private bool isWallGrab = false;

    //private
    // 붙을수 있는 벽에 붙거나 나갈때에 변경될 변수
    private bool exitWallGrab = false;


    //private
    // 벽에 붙은뒤 대각선 점프할때 필요한 변수
    private bool wallJump = false;
    private bool isleftWall = false;
    private bool isRightWall = false;

    //private
    // 벽에 붙은뒤 공격시 에러가 나기때문에 만든 변수
    private bool isAttackClingWallCoolTimeBool = false;

    //private
    // 23.08.20 Flip 했는지 처리할 변수
    private bool isFlipJump = false;

    //private
    // 23.08.21 WallGrab상태중 ASD 눌렀는지 확인 해줄 변수
    private bool wallGrabTouch = false;

    //private
    // 덤블링후 앉지 못하게 처리할 변수
    //  true상태일때 못하게 할거임
    private bool headOffPrecrouch = false;


    // 플레이어 데미지 받을수 있는 상태인지 체크하는 변수
    public bool isDodge = false;

    //private
    // 현재플레이어가Floor 에  땅에 닿아 있는지 알려줄 Bool변수
    private bool playerPresentFloor = false;

    // 왼쪽에서 맞음
    private bool leftHit;

    //  오른쪽에서 맞음 
    private bool rightHit;

    public bool isDie = false;

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
    Animator animator;
    BoxCollider2D playerBoxCollider;
    public GameObject leftAttackObj;

    Vector3 frontScale = Vector3.one;
    Vector3 backScale = new Vector3(-1f, 1f, 1f);
    Vector3 diePosition;

    #region 코루틴 캐싱 부분

    // ================= 코루틴 ======================

    private Coroutine leftAttackCoroutine;    // 코루틴 캐싱
    private Coroutine moveRock;
    private Coroutine addforceReset;
    private Coroutine rollRock;
    private Coroutine isAttackClingWallCoolTime;

    // 23.08.22추가 죽어서 AddForce로 힘을 주고 그대로 정지 시키게 하기 위해서코루틴 이용 예정
    private Coroutine playerDiePositionPick;

    // 23.08.21     10 : 55 리펙토링 하면서 코루틴 추가 점프 문제로 추가했음
    private Coroutine jumpCoroutine;
    private Coroutine exitWallGrabCoroutine;
    private Coroutine wallGrabToJumpCoroutine;
    private Coroutine flipExitCoroutine;


    // 아래 fixedUpdate는 0.02초 주기이기에 계산 잘해야할거같음
    public WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate(); // yield return fixedUpdate캐싱
    public WaitForSeconds waitSecond = new WaitForSeconds(0.3f);


    // ================= 코루틴 ======================

    #endregion 코루틴 캐싱

    #region 오디오 관련
    AudioSource audioSource;

    // [0] = Jump001 [1] = Jump002    [2] = Roll
    // [3] = Run001  [4] = Run002     [5] = Run003
    // [6] = Die     [7] = Landing001 [8] = FlipJump001

    [SerializeField] private AudioClip[] audioClip;
    #endregion 오디오 관련 

    public void Awake()
    {
        ghost = GetComponent<SG_Ghost>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        playerRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        // clip 이 비어있는상태에서 비교하거나 가져오면 오류가 뜨기때문에 처음에 clip만 삽입해주고 Play하지 않음
        if (audioSource.clip == default || audioSource.clip == null)
        {
            audioSource.clip = audioClip[5];
        }

        leftAttackObj.SetActive(false);
        FirstBoolFalse();


    }
    public void FixedUpdate()
    {

    }
    // Update is called once per frame

    //주기적으로 부르는 업데이트
    void Update()
    {
        // 죽지 않았을때에 돌게 만듦
        if (isDie == false)
        {
            LeftMove();
            RightMove();
            JumpMove();
            IsDown();
            IsRoll();
            WallGrabAtInput();
            AttackClick();
            if(player.GetAnyButton())
            {

                ghost.isGhostMake = true;
            }
            else
            {
                ghost.isGhostMake = false;
            }
        }

        //Debug.LogFormat("playerPresentFloor -> {0}", playerPresentFloor);

    }

    //========================================콜라이더=======================================

    #region        콜라이더



    // --------------------------------------Collision Enter------------------------------------
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 죽지 않은 상태일때에
        if (isDie == false)
        {
            // !플레이어가 무적이 아닐때
            if (isDodge == false)
            {


                
            }
            // !플레이어가 무적이 아닐때

            // 아래 변수를 조건을 조금 변경해야 함 SG_ClingWall이라는 벽이 아니라면 으로 조건걸고 넣어야함
            headOffPrecrouch = false;

            // ================================태그가 적일때 ========================================
            #region
            if (collision.gameObject.CompareTag("Enemy"))
            {

            }
            else
            {
                //attackCount = 0;
                // 23.08.21 09:36 점프 오류로 인한 임시 주석처리
                //isJump = false;
            }
            #endregion


           
            // ================================= 태그가 붙을수 있는 벽일때 SG_ClingWall =================================
            #region       

            // FlipJump 이후에 땅에 닿거나 붙을수 있는 벽을 다시 붙었을 경우 무적 해제
            //if(isFlipJump == true && isDodge == true && 
            //    (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("SG_ClingWall")))
            //{
            //    isDodge = false;
            //}

            if (collision.gameObject.CompareTag("SG_ClingWall") && isAttackClingWallCoolTimeBool == false)
            {
                // 23.08.21     09 : 40  Jump 고친후 WallGrab상태에서 Flip 점프 못가는것때문에 false로 주는것 추가
                isJump = false;
                
                exitWallGrab = false;
                isWallGrab = true;
                wallGrabCount = 1;

                #region 붙는 벽에따라 플레이어의 Scale을 조정하는 코드
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
                #endregion 붙는 벽에따라 플레이어의 Scale을 조정하는 코드

                animator.Play("WallGrab");
                // ?? 플레이어가 빨리 떨어지지 않게하기 위해 한거같음 ??
                if (playerRigid.gravityScale == 1 && playerRigid.mass > 0)
                {
                    playerRigid.gravityScale = 0.3f;
                    playerRigid.mass = 0.3f;
                }

                else { /*PASS*/ }


            }


            #endregion
            // 덤블링 후 바닥에 닿았을떄에 if

            // ================================= 태그가 바닥일때 Floor ================================================
            #region

            //Debug.LogFormat("무엇이든 콜라이더 Enter 일때 {0}", isJump);
            //  !점프 if
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform")) && isJump == true)
            {
                //여기서 다시 점프가능            
                isJump = false;

                //!오디오 착지소리
                // 땅의 포지션 Y보다 플레이어의 포지션 Y 가 더 클때만 소리 플레이
                if (collision.gameObject.transform.position.y < this.transform.position.y)
                {
                    audioSource.clip = audioClip[7];
                    audioSource.Play();
                }
                else { /*PASS*/ }

                animator.SetTrigger("Landing");
                playerPresentFloor = true;
                jumpCoroutine = StartCoroutine(LandingTriggerReset());

            }   //  !점프 if
            else { /*PASS*/ }

            // WallGrab 상태에서 ASD 키 눌렀을때 들어오게 하기 위한 if
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
                && isJump == false && wallGrabTouch == true)
            {
                animator.SetTrigger("Landing");
                wallGrabTouch = false;
                // 점프착지 트리거 초기화
                jumpCoroutine = StartCoroutine(LandingTriggerReset());

                // 벽붙기 트리거 초기화
                exitWallGrabCoroutine = StartCoroutine(ExitWallGrabTriggerReset());

                // 벽붙기 -> 점프 로 가는 트리거 초기화
                wallGrabToJumpCoroutine = StartCoroutine(WallGrabToJumpTriggerReset());

                //animator.ResetTrigger("ExitWallGrab");
            }
            else { /*PASS*/ }

            // FlipJump 를 한뒤에 땅에 닿으면 애니메이터 Exit 해주기위함
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
                && isFlipJump == true)
            {
                //!오디오 착지소리
                audioSource.clip = audioClip[7];
                audioSource.Play();

                animator.SetTrigger("FlipExit");
                isFlipJump = false;
            }
            else { /*PASS*/ }

            // 벽에서 흘러내려온다음 땅에 닿는다면
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
                && isWallGrab == true)
            {
                animator.SetTrigger("ExitWallGrab");
                isWallGrab = false;

                // 붙은 벽에서 흘러내려서 땅에 닿았을때 반대편을 벽에서 바라본방향으로 향하게 하는 로직
                if (this.transform.localScale == frontScale)
                {
                    this.transform.localScale = backScale;
                }
                else if (this.transform.localScale == backScale)
                {
                    this.transform.localScale = frontScale;
                }

            }
            else { /*PASS*/ }

            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform")) && attackCount > 0)
            {
                attackCount = 0;
                animator.SetTrigger("Landing");

                // 공격이후 땅에 붙었을때 켜지면 안돼는 Trigger들 초기화
                // 벽붙기 트리거 초기화
                exitWallGrabCoroutine = StartCoroutine(ExitWallGrabTriggerReset());
                // 점프착지 트리거 초기화
                jumpCoroutine = StartCoroutine(LandingTriggerReset());
                // FlipExit 트리거 초기화
                flipExitCoroutine = StartCoroutine(FlipExitTriggerReset());

            }

            #endregion



        }
    }   //OnCollisionEnter


    //------------------------------------------- Collision Stay ---------------------------------------------
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (isDie == false)
        {
            if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
            {
                // 천천히 내려오게 하기 위해 중력 0으로 설정
                this.gameObject.transform.position = this.gameObject.transform.position - new Vector3(0f, 0.001f, 0f);
            }
            else { /*PASS*/ }

            if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
            {
                // Floor 라는 바닥에 닿고 있는동안은 true
                playerPresentFloor = true;
            }


            //  땅에 닿아있는데 공격상태이고 Jump 애니메이션이 실행중이라면 강제로 Idle 애니메이션 으로 보냄
            if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform")) && isAttacking == true &&
               animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.Jump"))
            {
                //! 플레이어가 땅에 닿으면 그냥 Idle애니메이션으로 가게하지만 플렛폼이라면
                //  서로의 Y를 비교해서 플레이어의  Y축이 더 높을때만 Idle로 가도록해서 아래서 부딫칠떄는 Idle로 가는걸 막음
                if (collision.gameObject.CompareTag("Floor"))
                {
                    animator.Play("IdleAnimation");
                }

                else if (collision.gameObject.CompareTag("Platform"))
                {
                    if (collision.gameObject.transform.position.y < this.gameObject.transform.position.y)
                    {
                        animator.Play("IdleAnimation");
                    }
                }
                else { /*PASS*/ }

            }
            else { /*PASS*/ }
        }
    }   // OnCollisionStay

    //------------------------------------------ Collision Exit --------------------------------------------
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (isDie == false)
        {
            if(playerRigid!=null)
            { 
            if (playerRigid.gravityScale == 0)
            {
                playerRigid.mass = 1;
                playerRigid.gravityScale = 1;
            }
            else { /*PASS*/ }

            }
            if (collision.gameObject.CompareTag("SG_ClingWall"))
            {
                exitWallGrab = true;
            }
            else { /*PASS*/ }

            if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
            {
                //Debug.Log("Exit에서 False로 바꿈");
                playerPresentFloor = false;
            }

            //  나중에 이상한곳에서 false가 된다면 위에 mass,gravityScale 1로 만드는 조건문 안에 넣으면 될거같음
            isleftWall = false;
            isRightWall = false;


        }
    }


    #endregion 콜라이더

    public void Die()
    {
        if(isDie==false)
        {

        isDie = true;
        gameOverUi.SetActive(true);
        audioSource.clip = audioClip[6];
        audioSource.Play();

        // 죽는 애니메이션 재생
        animator.SetTrigger("DieTrigger");
        }

    }
    #region 트리거 콜라이더
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag.Equals("Dead"))
        {
            if(isDie==false)
            {

            isDie = true;
            gameOverUi.SetActive(true);
            audioSource.clip = audioClip[6];
            audioSource.Play();

            // 죽는 애니메이션 재생
            animator.SetTrigger("DieTrigger");
            }

        }
        if (isDie == false) //플레이어가 죽지않았을때
        {
            // !플레이어가 무적이 아닐때
            if (isDodge == false)
            {
                // 레이저에 맞아서 죽었을때에
                if (collision.gameObject.CompareTag("SG_LaserShot"))
                {

                    isDie = true;
                    gameOverUi.SetActive(true);
                    audioSource.clip = audioClip[6];
                    audioSource.Play();

                    // 죽는 애니메이션 재생
                    animator.SetTrigger("DieTrigger");

                    // X포지션이 양수로 날아가야함
                    if (this.gameObject.transform.position.x > collision.gameObject.transform.position.x)
                    {
                        leftHit = true;                        

                        playerDiePositionPick = StartCoroutine(PlayerDiePositionPick());

                        this.transform.localScale = backScale;

                    }
                    // X 포지션이 음수로 날아가야함
                    else if (this.gameObject.transform.position.x < collision.gameObject.transform.position.x)
                    {
                        rightHit = true;
                        
                        playerDiePositionPick = StartCoroutine(PlayerDiePositionPick());

                        this.transform.localScale = frontScale;
                    }
                }
                else{ /*PASS*/ }

                // 환풍기에 맞으면 
                //if(collision.gameObject.CompareTag("SG_Fan"))
                //{
                //    isDie = true;
                //    gameOverUi.SetActive(true);

                //    audioSource.clip = audioClip[6];
                //    audioSource.Play();

                //    // 죽는 애니메이션 재생
                //    animator.SetTrigger("DieTrigger");

                //    // X포지션이 양수로 날아가야함
                //    if (this.gameObject.transform.position.x > collision.gameObject.transform.position.x)
                //    {
                //        leftHit = true;

                //        playerDiePositionPick = StartCoroutine(PlayerDiePositionPick());

                //        this.transform.localScale = backScale;

                //    }
                //    // X 포지션이 음수로 날아가야함
                //    else if (this.gameObject.transform.position.x < collision.gameObject.transform.position.x)
                //    {
                //        rightHit = true;

                //        playerDiePositionPick = StartCoroutine(PlayerDiePositionPick());

                //        this.transform.localScale = frontScale;
                //    }
                //}
                //else { /*PASS*/ }
            }
        }
    }       // TriggerEnter


    #endregion 트리거 콜라이더


    //=======================================커스텀 함수=========================================

    private void FirstBoolFalse()
    {
        // 좌,우 누르고 땔때에 값이 변경되는 변수
        isRightMove = false;
        isLeftMove = false;

        // 애니메이터 Run을 위한 Bool 변수
        readyRun = false;

        //현재 플레이어가 점프한 상태인지 아닌지 구별하기 위한 변수
        // 점프는 Play로 할것이고 나가는것은 땅에 닿았을때에 Trigger로 줄것임
        isJump = false;

        //덤블링 중에 숙이지 못하게 하기위한 변수
        headOffPrecrouch = false;

        // S 라는 키를 눌렀는지 확인할 변수
        isDownInput = false;

        // 구르기 쿨타임인지 체크할 변수
        isRollRock = false;
        // 구르는 중인지를 알려줄 변수
        isRolling = false;

        // 현재 벽에 붙어있는지 확인할 변수
        isWallGrab = false;
        // 붙는벽 collider Exit 될떄에 true가 될 변수
        exitWallGrab = false;

        // 23.08.20 Flip 했는지 처리할 변수
        isFlipJump = false;

        // 무적 상태인지 체크할 변수
        isDodge = false;

        // 현재 플레이어가 땅에 닿아 있는지 체크할 변수
        // CollisionEnter = True CollisionExit = false
        playerPresentFloor = false;

        // 공격을 했는지 알려줄 변수
        isAttacking = false;
        // 현제 공격이 쿨타임인지 체크할 변수 쿨타임이 true일시 클릭해도 공격이 나가지 않음
        leftClickAttackCoolTime = false;

        // 벽점프를 했는지 알려줄 변수
        wallJump = false;

        // 내가 붙은 벽이 왼쪽에 있는 벽인지 오른쪽에 있는벽인지 알려줄 변수
        isleftWall = false;
        isRightWall = false;

        // 벽에 붙어서 공격후 다시 벽에 붙지 않게 해줄 변수
        isAttackClingWallCoolTimeBool = false;


    }


    #region 이동관련 커스텀함수
    public void LeftMove()
    {

        // 공격중이 아니고 구르는 중이 아닐때에
        if (isAttacking == false && isRolling == false)
        {

            if (player.GetButton("MoveLeft"))
            {
                // { 플레이어 중력 정상화 로직
                if (playerRigid.gravityScale < 1)
                {
                    playerRigid.gravityScale = 1f;
                    playerRigid.mass = 1f;
                }
                else { /*PASS*/ }
                // } 플레이어 중력 정상화 로직

                isLeftMove = true;

                // 이동 오디오 커스텀 함수
                MoveAudio();

                this.gameObject.transform.localScale = backScale;
                readyRun = true;
                Vector3 move = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;
                animator.SetBool("ReadyRun", readyRun);
            }

            if (player.GetButtonUp("MoveLeft") && isRightMove == true)
            {
                isLeftMove = false;
            }
            else if (player.GetButtonUp("MoveLeft") && isRightMove == false)
            {
                isLeftMove = false;
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);
            }

        }
        if(player.GetButtonUp("MoveLeft")&& playerPresentFloor)
        {
            playerRigid.velocity = Vector2.zero;
        }

    }

    public void RightMove()
    {

        if (isAttacking == false && isRolling == false)
        {

            if (player.GetButton("MoveRight"))
            {
                // { 플레이어 중력 정상화 로직
                if (playerRigid.gravityScale < 1)
                {
                    playerRigid.gravityScale = 1f;
                    playerRigid.mass = 1f;
                }
                else { /*PASS*/ }
                // } 플레이어 중력 정상화 로직

                isRightMove = true;

                // 이동 오디오 커스텀 함수
                MoveAudio();

                this.gameObject.transform.localScale = frontScale;
                readyRun = true;
                Vector3 move = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
                playerRigid.velocity = move;

                animator.SetBool("ReadyRun", readyRun);

            }
            if (player.GetButtonUp("MoveRight") && isLeftMove == true)
            {
                isRightMove = false;
            }

            else if (player.GetButtonUp("MoveRight") && isLeftMove == false)
            {
                isRightMove = false;
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);
            }
        }
        if (player.GetButtonUp("MoveRight") && playerPresentFloor)
        {
            playerRigid.velocity = Vector2.zero;
        }

    }
    public void IsDown()
    {

        if (player.GetButtonDown("Down") && headOffPrecrouch == false)
        {
            isDownInput = true;
            animator.SetBool("PrecrouchBool", isDownInput);
            playerBoxCollider.offset = new Vector2(0.07f, -0.27f);
            playerBoxCollider.size = new Vector2(0.55f, 0.64f);
        }
        else { /*PASS*/ }

        if (player.GetButton("Down") && headOffPrecrouch == false)
        {
            isDownInput = true;
        }

        if (player.GetButtonUp("Down") && headOffPrecrouch == false)
        {
            isDownInput = false;
            animator.SetBool("PrecrouchBool", isDownInput);

            playerBoxCollider.offset = new Vector2(0.07f, -0.03f);
            playerBoxCollider.size = new Vector2(0.55f, 1.1f);
        }
        else { /*PASS*/ }
    }

    public void IsRoll()
    {
        if (isJump == false && isRollRock == false && isWallGrab == false)
        {
            if (isRightMove == true && isDownInput == true
                && isLeftMove == false)
            {
                isDodge = true;

                rollRock = StartCoroutine(RollCoolTime());
                animator.Play("PlayerRoll");

                // 달리기 끄는 로직
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);
                dodgeReturn = false;



            }       // if: 오른쪽 구르기인지?
            else { /*PASS*/ }

            if (isLeftMove == true && isDownInput == true &&
                isRightMove == false)
            {
                isDodge = true;

                rollRock = StartCoroutine(RollCoolTime());
                animator.Play("PlayerRoll");

                // 달리기 끄는 로직
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);
                dodgeReturn = false;

            }       // if: 왼쪽 구르기인지?
            else { /*PASS*/ }


        }

    }

    public void JumpMove()
    {

        if (player.GetButtonDown("Jump") && isJump == false && isWallGrab == false)
        {
            animator.Play("Jump");
            if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
            {
                playerRigid.mass = 1;
                playerRigid.gravityScale = 1;
            }
            //점프 했을때에 또 점프 못하게 bool 처리
            isJump = true;

            // !오디오 (점프)
            audioSource.clip = audioClip[2];
            audioSource.Play();

            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            headOffPrecrouch = true;

        }
        else { /*PASS*/ }
        //  붙는 벽에 달라붙은 상태에서 점프를 했을경우
        if (player.GetButtonDown("Jump") && isJump == false && isWallGrab == true)
        {
            FlipJump();
        }
        else { /*PASS*/ }
    }

    // 대각선 점프
    public void FlipJump()
    {
        isDodge = true;
        if (isleftWall == true)
        {
            //  점프전에 중력 정상화
            if (playerRigid.mass < 1 || playerRigid.gravityScale < 1)
            {
                playerRigid.mass = 1;
                playerRigid.gravityScale = 1;
            }

            // !Check 08.20 이거 필요한지 확인해야함
            wallJump = true;

            isFlipJump = true;

            //!오디오 (벽점프)
            audioSource.clip = audioClip[8];
            audioSource.Play();

            animator.SetTrigger("FlipStart");
            this.gameObject.transform.localScale = backScale;
            playerRigid.AddForce(new Vector2(8f, 9f), ForceMode2D.Impulse);

            this.gameObject.transform.localScale = frontScale;

            isJump = true;
            isWallGrab = false;

            // !Check 08.20 이거 필요한지 확인해야함       
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

            isFlipJump = true;

            //!오디오 (벽점프)
            audioSource.clip = audioClip[8];
            audioSource.Play();

            animator.SetTrigger("FlipStart");
            this.gameObject.transform.localScale = frontScale;
            playerRigid.AddForce(new Vector2(-8f, 9f), ForceMode2D.Impulse);


            this.gameObject.transform.localScale = backScale;


            isJump = true;
            isWallGrab = false;
            headOffPrecrouch = true;

        }
    }

    // Wall Grab상태일때에 키를 눌른다면
    public void WallGrabAtInput()
    {

        //  { if : 벽에붙어있으며 W키가아닌 A,S,D 키중 하나라도 눌렀을경우 
        if (isWallGrab == true && isFlipJump == false &&
            (player.GetButtonDown("MoveLeft") || player.GetButtonDown("MoveRight") || player.GetButtonDown("Down")))
        {
            // 쿨타임을 주어서 벽에서 붙은판정으로 가지 못하게 막음

            // 23.08.20 애니메이션 WallGrab에서 Jump로 가도록 해서 떨어지는 듯한 느낌을 줌
            // Test 해보았는데 여길 잘못건들이면 플레이어가 망가져 버리는거같음
            //isJump = true;
            //animator.SetTrigger("WallGrabToJump");

            exitWallGrab = true;

            // 23.08.21     11:04 다시 시도

            //아래 디버그 잘 들어가는것 확인됨 W 눌렀을때는 안들어옴
            //Debug.Log("벽을 잡고 있을때에 ASD 키를 누르면 들어오나?");
            animator.SetTrigger("WallGrabToJump");
            wallGrabTouch = true;

            // 여기에 갔을때에 는 Landing Trigger 가 정상 작동 하지 않음

            // 여기에 코루틴 만들어서 ReSetTrigger 넣어줄 생각
            //jumpCoroutine = null;
            // jumpCoroutine = StartCoroutine()

            // !TODO : 캐싱한것 사용하게 만들어야겠음
            StartCoroutine(IsAttackClingWallCoolTime());
            //exitWallGrab = false;

        }   //  } if : 벽에붙어있으며 W키가아닌 A,S,D 키중 하나라도 눌렀을경우 

    }



    #endregion

    #region 오디오 관련 커스텀 함수
    private void MoveAudio()
    {

        // if : (1)플레이어가 A or D 키를 누른 상태여야하고 (2)audio가 플레이 중이지 않아야하고 
        //      (3)플레이어가 땅에 닿아 있어야 들어감
        if (!audioSource.isPlaying && playerPresentFloor == true &&
            (isRightMove == true || isLeftMove == true))
        {
            //Debug.LogFormat("audioSource.clip 이 존재 하는지?? {0}", audioSource.clip != null);
            //Debug.LogFormat("audioSource.clip은 null이라서 이름도 못찾음. -> {0}", audioSource.clip.name);


            // if : 오디오가 순차적으로 재생 되도록 하고싶음            
            if (audioSource.clip.name == ("Run002"))
            {
                // audioClip[5] = Run003
                audioSource.clip = audioClip[5];
                audioSource.Play();
            }
            else if (audioSource.clip.name != ("Run002"))
            {
                audioSource.clip = audioClip[4];
                audioSource.Play();
            }
            else { /*PASS*/ }
            //Debug.LogFormat("Clip Name -> {0}", audioSource.clip.name);
        }
        else { /*PASS*/ }
    }

    #endregion 오디오 관련 커스텀 함수

    public void AttackClick()
    {
        //  좌클릭시
        if (Input.GetMouseButtonDown(0))
        {
            ghost.isGhostMake = true;
            if (leftClickAttackCoolTime == false && attackCount < 3) // 좌클릭 쿨타임 아닐때에 실행됨                        
            {
                //// 벽붙기 트리거 초기화
                //exitWallGrabCoroutine = StartCoroutine(ExitWallGrabTriggerReset());

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

                // 23.08.21 이쯤에서 공격 애니메이션 재생 하면 될거같음
                animator.Play("AttackAnimaition001");

                leftAttackObj.SetActive(true);
                one = 1;
                attackCount += 1;
                readyRun = false;
                isAttacking = true;
                leftAttackCoroutine = StartCoroutine(LeftClickAttack());
                moveRock = StartCoroutine(MoveRock());

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
                    playerRigid.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
                }
                else if (mousePosition.y < gameObject.transform.position.y)
                {
                    playerRigid.AddForce(new Vector2(0f, -5f), ForceMode2D.Impulse);
                }
                else { /*PASS*/ }


                // 특정한 시점에서 공격후 달리기 떄문에 달리기 끄는 로직 추가
                // 달리기 끄는 로직
                readyRun = false;
                animator.SetBool("ReadyRun", readyRun);

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

            playerRigid.velocity = Vector3.zero;
        ////  플레이어보다 앞을 눌렀을때에
        //if (mousePosition.x > gameObject.transform.position.x)
        //{
        //}
        //else if (mousePosition.x < gameObject.transform.position.x)
        //{
        //    playerRigid.velocity = Vector3.one * -1;
        //}
    }
    // } 공격후 AddForce의 힘값을 일정시간 이후 초기화


    // { 구르기 한뒤에 쿨타임을 줄예정
    private IEnumerator RollCoolTime()
    {

        isRollRock = true;
        isRolling = true;
        // 여기서 구를 때에 앞으로 힘을 가해주면 될거같은데?

        if (isRightMove == true && isJump == false) // 오른쪽 구르기
        {
            // !오디오 (구르기)
            audioSource.clip = audioClip[2];
            audioSource.Play();

            playerRigid.AddForce(new Vector3(8f, 0f, 0f), ForceMode2D.Impulse);

            for (int i = 0; i <= 10; i++)
            {
                yield return fixedUpdate;
            }
            if(dodgeReturn)
            {
                yield break;
            }
            playerRigid.velocity = Vector3.zero;
            isRolling = false;
            animator.SetTrigger("RollEnd");
            isDodge = false;
            for (int j = 0; j <= 17; j++)
            {
                yield return fixedUpdate;
            }
            isRightMove = false;
            
        }   // 오른쪽 구르기

        if (isLeftMove == true && isJump == false) // 왼쪽 구르기
        {
            // !오디오 (구르기)
            audioSource.clip = audioClip[2];
            audioSource.Play();

            playerRigid.AddForce(new Vector3(-8f, 0f, 0f), ForceMode2D.Impulse);
            for (int i = 0; i <= 10; i++)
            {
                yield return fixedUpdate;
            }
            if (dodgeReturn)
            {
                yield break;
            }
            playerRigid.velocity = Vector3.zero;
            isRolling = false;
            animator.SetTrigger("RollEnd");
            isDodge = false;
            for (int j = 0; j <= 17; j++)
            {
                yield return fixedUpdate;
            }
            isLeftMove = false;
            
        }   // 왼쪽 구르기
        isRollRock = false;

    }
    // } 구르기 한뒤에 쿨타임을 줄예정

    private IEnumerator IsAttackClingWallCoolTime()
    {
        yield return waitSecond;
        isAttackClingWallCoolTimeBool = false;
        exitWallGrab = false;
    }

    // 죽었을때에 포지션을 공중에 고정 시키기위한 코루틴
    private IEnumerator PlayerDiePositionPick()
    {

        for(int i=0; i <= 5; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        

        // 왼쪽에서 맞았는지 체크
        if (leftHit == true && rightHit == false)
        {
            playerRigid.velocity = Vector3.zero;
            playerRigid.AddForce(new Vector2(5f, 5f), ForceMode2D.Impulse);
            leftHit = false;
        }
        // 오른쪽에서 맞았는지 체크
        else if (leftHit == false && rightHit == true)
        {
            playerRigid.velocity = Vector3.zero;
            playerRigid.AddForce(new Vector2(-5f, 5f), ForceMode2D.Impulse);
            rightHit = false;
        }
        else { /*PASS*/ }

        // 여기에 좌표를 저장하고 계속 포지션을 갱신 시켜줘야함
        for (int i = 0; i <= 25; i++)
        {
            yield return fixedUpdate;
        }
        
        diePosition = this.transform.position;

        // !23.08.22 임시로 해둠 추후 고쳐야할수도 있음

        playerRigid.gravityScale = 0f;
        playerRigid.mass = 0f;


        // TODO : 아래 for문을 시간 되돌리기 전까지 돌도록 하면 될거같음
        for (int j = 0; j <= 1000; j++)
        {
            this.transform.position = diePosition;
            yield return fixedUpdate;
        }        
        // !23.08.22 임시로 해둠 추후 고쳐야할수도 있음

    }
    // 아래는 트리거 초기화 해줄 코루틴

    private IEnumerator LandingTriggerReset()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return fixedUpdate;
        }
        animator.ResetTrigger("Landing");
    }

    private IEnumerator ExitWallGrabTriggerReset()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return fixedUpdate;
        }
        animator.ResetTrigger("ExitWallGrab");
    }

    private IEnumerator WallGrabToJumpTriggerReset()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return fixedUpdate;
        }
        animator.ResetTrigger("WallGrabToJump");
    }

    private IEnumerator FlipExitTriggerReset()
    {
        for (int i = 0; i <= 2; i++)
        {
            yield return fixedUpdate;
        }
        animator.ResetTrigger("FlipExit");
    }

}   // NameSpace
