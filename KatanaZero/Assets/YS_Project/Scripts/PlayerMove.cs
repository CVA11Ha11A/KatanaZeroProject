using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMove : MonoBehaviour
{
    Player player;
    int playerId = 0;
    public float moveSpeed = 3f;
    private float jumpForce = 6f;
    private bool isRun;
    private bool isJump;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButton("PlayerLeft"))
        {
            isRun = true;
            ghost.isGhostMake = true;
            Vector3 movement = new Vector3(-moveSpeed, playerRigid.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            playerRigid.velocity = movement;
            
        }
        else if (player.GetButton("PlayerRight"))
        {
            ghost.isGhostMake = true;

            isRun = true;
            Vector3 movement = new Vector3(moveSpeed, playerRigid.velocity.y, 0f);
            transform.localScale = new Vector3(1f, 1f, 1f);

            playerRigid.velocity = movement;
            

        }
        else
        {
            ghost.isGhostMake = false;

            isRun = false;
           // ghost.isGhostMake = false;

        }
        playerAni.SetBool("Run", isRun);
        if (player.GetButtonDown("PlayerJump"))
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
            ghost.isGhostMake = true;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Floor"))
        {
            isJump = false;
        }
    }

}
