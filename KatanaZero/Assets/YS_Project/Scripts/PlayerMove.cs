using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using static PlayerMove;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerState
    {
       Intro,Idle,Run,Jump
    }
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
    private float playerScale;
    Rigidbody2D playerRigid;
    Animator playerAni;
    Ghost ghost;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        player = ReInput.players.GetPlayer(playerId);
        playerRigid = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        ghost = FindAnyObjectByType<Ghost>();
       
       StartCoroutine(Intro());
        
    }

    // Update is called once per frame
    void Update()
    {
        playerScale = transform.localScale.x;
       isWall= Physics2D.Raycast(wallCheck.position, Vector2.right * playerScale, wallCheckDis, wall_mask);
       
        if (IntroCanvas.isIntroOver == false)
        {
            if (state == PlayerState.Intro)
            {
                ghost.isGhostMake = false;

            }
            return;
        }
        if (player.GetButton("MoveLeft"))
        {
            isRun = true;
            ghost.isGhostMake = true;
            Vector3 movement = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            state = PlayerState.Run;

            playerRigid.velocity = movement;
           


        }
        else if (player.GetButton("MoveRight"))
        {
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
           if(isJump == false)
            {
                
                isJump = true;
            }
           else
            {
                return;
            }
            playerRigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        }
        //if(isWallJump)
        //{
        //    Vector3 wallJump = new Vector3(-10f, jumpForce, 0f);
        //    playerRigid.AddForce(wallJump, ForceMode2D.Impulse);
        //    transform.localScale = new Vector3(-1, 1, 1);
        //}
        if(isJump==true)
        {
            state = PlayerState.Jump;
            ghost.isGhostMake = true;
        }
         
        if(state==PlayerState.Idle&&isStair==true)
        {
            playerRigid.velocity = new Vector2(0,0);
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
        if(isWall)
        {
            isWallJump = false;
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, playerRigid.velocity.y * 0.9f);
            if(isWallJump==false)
            if(player.GetButton("Jump"))
            {
                isWallJump = true;
                Invoke("FreezeX", 0.3f);
                playerRigid.velocity = new Vector2(-playerScale * 5f, 0.9f * 5f);
                if(transform.localScale.x==1)
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
    void FreezeX()
    {
        isWallJump = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Floor")||collision.collider.tag.Equals("Platform"))
        {
            isJump = false;
        }
        if (collision.collider.tag.Equals("Stair"))
        {
            isJump = false;
            isStair = true;
        }
        if(collision.collider.tag.Equals("Wall"))
        {
            //isJump = false;
            if(player.GetButtonDown("Jump"))
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
    }
    private IEnumerator Intro()
    {
        GameManager manager = FindAnyObjectByType<GameManager>();
        manager.IntroAction();
        playerAni.SetTrigger("PlaySong");
        yield return new WaitForSeconds(3);
        state = PlayerState.Idle;
    }

}
