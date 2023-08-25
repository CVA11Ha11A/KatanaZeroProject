using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using static PlayerMove;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.EventSystems;


public class PlayerMove : MonoBehaviour
{
    public enum PlayerState
    {
        Intro, Idle, Run, Jump, Attack,Crouch
    }
    #region Public 변수
    public bool dodgeReturn=false;
    public GameObject gameOverUi;
    public GameObject slash;
    public Transform wallCheck;
    public float wallCheckDis;
    public LayerMask wall_mask;
    public PlayerState state;
    public IntroCanvas introCan;
    public GameObject rollDust;
    public float moveSpeed = 3f;
    public float attackDuration = 0.2f; // ���� ���� �ð�
    public float attackSpeed = 5f; // ���� �� ������ �ӵ�
    public float attackCooldown = 1f; // ���� ��ٿ�
    public bool isDodge = false;
    public bool isDie = false;
    public AudioClip deathClip;
    #endregion
    private float jumpForce = 7f;
    private bool isRun;
    private bool isJump;
    private bool isStair;
    private AudioSource deathSound;
    private bool isWallJump;
    private bool isWall;
    private bool isAttacking;
    private float playerScale;
    private float direction;
    private float wallJumpTimer=0;
    private float wallJumpRate = 0.2f;
    private bool isGrounded;
    private float moveDirection;
    private float jumpTimer = 0;
    private float jumpRate = 0.2f;
    private CapsuleCollider2D playerCollider;
    private float rollTimer = 0;
    private float rollRate = 0.3f;
    private int attackCount = 0;
    private float lastAttackTime;
    Rigidbody2D playerRigid;
    Animator playerAni;
    Ghost ghost;
    Player player;
    int playerId = 0;
    SoundManager soundManager;
    IntroManager introManager;
    Vector2 targetPosition;
    Vector3 leftScale=new Vector3(-1f, 1f, 1f);
    Vector3 rightScale= new Vector3(1f, 1f, 1f);
    // Start is called before the first frame update
    private void Awake()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        
    }
    void Start()
    {
        introManager = FindAnyObjectByType<IntroManager>();
        player = ReInput.players.GetPlayer(playerId);
        playerRigid = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        ghost = FindAnyObjectByType<Ghost>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        introCan = FindAnyObjectByType<IntroCanvas>();
        deathSound = GetComponent<AudioSource>();
        if (state == PlayerState.Intro)
        {
            if(introManager!=null)
            {
                if(introManager.introOver==false)
                {
            StartCoroutine(Intro());

                }
            }

        }



    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded&&state==PlayerState.Idle)
        {
            playerRigid.velocity = Vector2.zero;
        }
        if(isDie)
        {
            playerAni.Play("PlayerDie");

            return;
        }
        playerScale = transform.localScale.x;
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * playerScale, wallCheckDis, wall_mask);


       if(introManager!=null)
        {

            if (state == PlayerState.Intro&&introManager.introOver==false)
            {
                ghost.isGhostMake = false;
                
            return;
            }
        }
          
        
        if ((player.GetButton("Down") && player.GetButtonDown("MoveLeft") && isGrounded)
            || (player.GetButtonDown("Down") && player.GetButton("MoveLeft") && isGrounded))
        {
            if (isDodge == false)
            {
                dodgeReturn = false;
                playerRigid.velocity = Vector2.zero;
                transform.localScale = leftScale;
                direction = -1;
                isDodge = true;
                Vector2 rollMoveLeft = new Vector2(direction * 12, playerRigid.velocity.y);
                playerRigid.AddForce(rollMoveLeft, ForceMode2D.Impulse);
                playerAni.Play("PlayerRoll");
            }
        }
        else if ((player.GetButton("Down") && player.GetButtonDown("MoveRight") && isGrounded)
            || (player.GetButtonDown("Down") && player.GetButton("MoveRight") && isGrounded))
        {

            if (isDodge == false)
            {
                playerRigid.velocity = Vector2.zero;
                transform.localScale = rightScale;
                direction = 1;
                isDodge = true;
                Vector2 rollMoveRight = new Vector2(direction * 12, playerRigid.velocity.y);
                playerRigid.AddForce(rollMoveRight, ForceMode2D.Impulse);
                playerAni.Play("PlayerRoll");
            }
        }

        if (state == PlayerState.Idle)
        {
            ghost.isGhostMake = false;

        }
        else
        {
            ghost.isGhostMake = true;

        }
        if (isDodge == true)
        {
            if(isWallJump)
            {
                return;
            }
            if(dodgeReturn)
            {
                StartCoroutine(DodgeReset());
                return; }
            ghost.isGhostMake = true;

            playerRigid.gravityScale = 3f;
            rollTimer += Time.deltaTime;
            if (rollTimer >= rollRate)
            {
                rollTimer = 0;
                isDodge = false;
                // playerRigid.velocity = Vector2.zero;
                rollDust.SetActive(false);
                playerRigid.velocity = Vector2.zero;
                playerRigid.gravityScale = 1f;
            }
            else
            {
                rollDust.SetActive(true);

            }
            return;
        }


        if (player.GetButtonDown("MoveLeft"))
        {
            moveDirection = -1;
           

        }
        else if (player.GetButtonDown("MoveRight"))
        {
            moveDirection = 1;
           


        }
        else if(player.GetButtonDown("Down") && isGrounded)
        {
            state = PlayerState.Crouch;
            playerAni.Play("PlayerCrouch");
        }

        if (player.GetButtonUp("Down") && isGrounded)
        {
            playerAni.Play("PlayerPostCrouch");
        }
        
        if (player.GetButton("MoveLeft"))
        {
            if (isWallJump)
            {
                return;
            }
            if (isWall && transform.localScale.x == -1)
            {
                isRun = false;
                return;
            }
            isRun = true;
            ghost.isGhostMake = true;
            Vector3 movement = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
            transform.localScale = leftScale;
            state = PlayerState.Run;
            playerRigid.velocity = movement;
            if(isGrounded && isJump == false)
            {

            playerAni.Play("PlayerRun");
            }
            

        }
        else if (player.GetButton("MoveRight"))
        {
            if (isWallJump)
            {
                return;
            }
            if (isWall && transform.localScale.x == 1)
            {
                isRun = false;

                return;
            }
            ghost.isGhostMake = true;

            isRun = true;
            Vector3 movement = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
            transform.localScale = rightScale;
            state = PlayerState.Run;

            playerRigid.velocity = movement;
            if (isGrounded && isJump == false)
            {

            playerAni.Play("PlayerRun");
            }

        }
        else if (player.GetButton("Down")&&isGrounded)
        {
            playerAni.Play("PlayerCrouchHold");

        }
        else
        {

            ghost.isGhostMake = false;

            isRun = false;
            // ghost.isGhostMake = false;

            state = PlayerState.Idle;





        }
        if (player.GetButtonUp("moveright") && isGrounded || player.GetButtonUp("moveleft") && isGrounded)
        {
            if(isWall==false)
            {
                playerAni.Play("PlayerRuntoIdle");
            Vector2 stopvelocity = new Vector2(0, playerRigid.velocity.y);
            playerRigid.velocity = stopvelocity;
            }
           
        }
       

        if (player.GetButtonDown("Jump")&&!isWall&&!isWallJump)
        {
            if (isJump == false)
            {

                isJump = true;
                playerAni.Play("PlayerJump");
            }
            else
            {
                return;
            }
            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        }
        if(isJump==true&& isWall==false&&isWallJump==false)
        {
            jumpTimer += Time.deltaTime;
            if(jumpTimer>=0.5f)
            {
               
                playerAni.Play("PlayerFall");
               // playerRigid.gravityScale = 2f;
                
            }
        }
        if (isJump == false && isGrounded == false && isWall == false&&isWallJump==false&&isAttacking==false&&isStair==false)
        {
            playerAni.Play("PlayerFall");

        }
        else if (isGrounded)
        {
            jumpTimer = 0;

            
        }



        if (!isAttacking)
        {

            if (player.GetButtonDown("Attack") && attackCount < 4)
            {
                
                soundManager.AttackSound();
                isAttacking = true;
                attackCount += 1;
                playerAni.Play("PlayerAttack");
                state = PlayerState.Attack;
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 attackDirection = (mouseWorldPos - transform.position).normalized;

                Vector2 modifiedForce = new Vector2(attackDirection.x * 20f, attackDirection.y * (20f / attackCount));
                playerRigid.velocity = Vector2.zero;
                playerRigid.angularVelocity = 0;
                slash.transform.position = this.transform.position;
                slash.SetActive(true);
                playerRigid.AddForce(modifiedForce, ForceMode2D.Impulse);
                if (attackDirection.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (attackDirection.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);

                }
                StartCoroutine(AttackGravity());
                StartCoroutine(AttackCoolDown());
            }

        }
        else
        {
            ghost.isGhostMake = true;

        }


        if(isWallJump)
        {
            ghost.isGhostMake = true;

        }
        if (isJump == true)
        {
            state = PlayerState.Jump;
            ghost.isGhostMake = true;
        }

        if (state == PlayerState.Idle && isStair == true)
        {
            playerRigid.velocity = new Vector2(0, 0);
            playerRigid.gravityScale = 0;

        }
        else
        {
            playerRigid.gravityScale = 1.2f;

        }
        //Debug.LogFormat("isWall{0}", isWall);
    }//Update()

    private void FixedUpdate()
    {
        
        if (isWall)
        {
            if(isGrounded==false)
            {
            playerAni.Play("Player_WallSlide");

            }
            isWallJump = false;
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, playerRigid.velocity.y * 0.9f);
            wallJumpTimer += Time.deltaTime;
            if(wallJumpTimer>=wallJumpRate)
            {
                if (isWallJump == false)
                if (player.GetButton("Jump"))
                {
                    playerAni.Play("Player_Flip");
                    isWallJump = true;
                        isDodge = true;
                    Invoke("FreezeX", 0.3f);
                    
                    playerRigid.velocity = new Vector2(-playerScale * 10f, 0.9f * 8f);
                    if (transform.localScale.x == 1)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else if (transform.localScale.x == -1)
                    {
                        transform.localScale = new Vector3(1, 1, 1);

                    }
                }
            }

        }
        else
        {
            wallJumpTimer = 0;
        }
    }
    void FreezeX()
    {
        isWallJump = false;
        isDodge = false;
    }
    public void Die()
    {
        gameOverUi.SetActive(true);
        deathSound.clip = deathClip;
        deathSound.Play();
        isDie = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead"))
        {
            if(isDie==false)
            {
            Die();

            }
        }
        if(collision.CompareTag("SG_Fan"))
        {
            dodgeReturn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SG_Fan"))
        {
            dodgeReturn = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.collider.tag.Equals("Floor") || collision.collider.tag.Equals("Platform"))
        {
            playerRigid.gravityScale = 1f;
            playerAni.Play("PlayerAnimation");
            isGrounded = true;
            attackCount = 0;
            isJump = false;
        }
        if (collision.collider.tag.Equals("Stair"))
        {
            attackCount = 0;
            isJump = false;
            playerAni.Play("PlayerAnimation");
            isGrounded = true;
            isStair = true;
        }
        if (collision.collider.tag.Equals("Wall"))
        {
            attackCount = 0;
            //isJump = false;
            if (player.GetButtonDown("Jump"))
            {
                isWallJump = true;
            }
        }
        if(collision.collider.CompareTag("Enemy"))
        {
            CapsuleCollider2D enemyCollider = collision.gameObject.GetComponent<CapsuleCollider2D>();
            if(enemyCollider!=null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Floor") || collision.collider.tag.Equals("Platform"))
        {
            isGrounded = true;
            attackCount = 0;

        }
        if (collision.collider.tag.Equals("Stair"))
        {
            isGrounded = true;

            attackCount = 0;

        }
        if (collision.collider.tag.Equals("Wall"))
        {
            attackCount = 0;
            //isJump = false;
            if (player.GetButtonDown("Jump"))
            {
                isWallJump = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Stair"))
        {

            isStair = false;
            if(isGrounded==false)
            {
            //playerRigid.gravityScale = 3f;

            }
        }
        if (collision.collider.tag.Equals("Floor") || collision.collider.tag.Equals("Platform"))
        {
            isGrounded = false;
           
        }
    }
    private IEnumerator Intro()
    {
        IntroManager manager = FindAnyObjectByType<IntroManager>();
        manager.IntroAction();
        Debug.Log("intro재생되는가");
        playerAni.Play("PlayerMusicPlay");
        yield return new WaitForSeconds(3);
        state = PlayerState.Idle;
    }

    private IEnumerator AttackGravity()
    {
        yield return new WaitForSeconds(0.3f);
        playerRigid.velocity = Vector3.zero;
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.3f);
        isDodge = false;
        isAttacking = false;
    }
   
    private IEnumerator DodgeReset()
    {
        yield return new WaitForSeconds(0.5f);
        isDodge = false;
    }
}
