using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using static PlayerMove;
using Unity.VisualScripting;
using TMPro;


public class PlayerMove : MonoBehaviour
{
    public enum PlayerState
    {
        Intro, Idle, Run, Jump, Attack
    }
    public GameObject slash;
    public Transform wallCheck;
    public float wallCheckDis;
    public LayerMask wall_mask;
    public PlayerState state;
    Player player;
    int playerId = 0;
    public float moveSpeed = 3f;
    private float jumpForce = 6f;
    private bool isRun;
    private bool isJump;
    private bool isStair;
    private bool isWallJump;
    private bool isWall;
    private bool isAttacking;
    private float playerScale;
    private float direction;
    private float wallJumpTimer=0;
    private float wallJumpRate = 0.2f;
    private bool isGrounded;
    Rigidbody2D playerRigid;
    Animator playerAni;
    Ghost ghost;
    public IntroCanvas introCan;
    private BoxCollider2D playerCollider;

    public float attackDuration = 0.2f; // ���� ���� �ð�
    public float attackSpeed = 5f; // ���� �� ������ �ӵ�

    public float attackCooldown = 1f; // ���� ��ٿ�
    private int attackCount = 0;
    private float lastAttackTime;
    Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {

        player = ReInput.players.GetPlayer(playerId);
        playerRigid = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        ghost = FindAnyObjectByType<Ghost>();
        playerCollider = GetComponent<BoxCollider2D>();


        if (state == PlayerState.Intro)
        {
            StartCoroutine(Intro());

        }



    }

    // Update is called once per frame
    void Update()
    {
        playerScale = transform.localScale.x;
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * playerScale, wallCheckDis, wall_mask);


        if (introCan.isIntroOver == false)
        {
            if (state == PlayerState.Intro)
            {
                ghost.isGhostMake = false;

            }
            return;
        }

        if (state == PlayerState.Idle)
        {
            ghost.isGhostMake = false;

        }
        else
        {
            ghost.isGhostMake = true;

        }
        if (player.GetButton("MoveLeft"))
        {
            if(isWallJump)
            {
                return;
            }
            if(isWall&&transform.localScale.x==-1)
            {
                isRun = false;
                return;
            }
            isRun = true;
            ghost.isGhostMake = true;
            Vector3 movement = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            state = PlayerState.Run;

            playerRigid.velocity = movement;



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
            transform.localScale = new Vector3(1f, 1f, 1f);
            state = PlayerState.Run;

            playerRigid.velocity = movement;


        }
        else
        {

            ghost.isGhostMake = false;

            isRun = false;
            // ghost.isGhostMake = false;

            state = PlayerState.Idle;





        }
        playerAni.SetBool("Run", isRun);
        if (player.GetButtonDown("Jump"))
        {
            if (isJump == false)
            {

                isJump = true;
            }
            else
            {
                return;
            }
            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        }
        
        if (!isAttacking)
        {

            if (player.GetButtonDown("Attack") && attackCount < 4)
            {
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


        //if(isWallJump)
        //{
        //    Vector3 wallJump = new Vector3(-10f, jumpForce, 0f);
        //    playerRigid.AddForce(wallJump, ForceMode2D.Impulse);
        //    transform.localScale = new Vector3(-1, 1, 1);
        //}
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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Floor") || collision.collider.tag.Equals("Platform"))
        {
            isGrounded = true;
            attackCount = 0;
            isJump = false;
        }
        if (collision.collider.tag.Equals("Stair"))
        {
            attackCount = 0;
            isJump = false;
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
            BoxCollider2D enemyCollider = collision.gameObject.GetComponent<BoxCollider2D>();
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
            attackCount = 0;

        }
        if (collision.collider.tag.Equals("Stair"))
        {
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
        }
        if (collision.collider.tag.Equals("Floor") || collision.collider.tag.Equals("Platform"))
        {
            isGrounded = false;
           
        }
    }
    private IEnumerator Intro()
    {
        GameManager manager = FindAnyObjectByType<GameManager>();
        manager.IntroAction();
        playerAni.SetTrigger("PlaySong");
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
        isAttacking = false;
    }

}
