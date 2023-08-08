using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerState
    {
       Intro,Idle,Run,Jump
    }
    PlayerState state;
    Player player;
    int playerId = 0;
    public float moveSpeed = 3f;
    private float jumpForce = 6f;
    private bool isRun;
    private bool isJump;
    private bool isStair;
    Rigidbody2D playerRigid;
    Animator playerAni;
    Ghost ghost;
    
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
        if(state==PlayerState.Intro)
        {
            ghost.isGhostMake = false;
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
    }//Update()
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
