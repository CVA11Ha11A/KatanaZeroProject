using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private bool isJump = false;
    private bool readyRun = false;
    private bool leftClickAttack = false;
    private Vector3 mousePosition;

    Player player;
    Rigidbody2D playerRigid;
    Animator playerAni;

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
        playerAni.SetBool("IsJumpBool", isJump);

        //Debug.LogFormat("어딘가에 부딫쳤다. Jump: {0}", isJump);
    }

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
            playerAni.SetTrigger("LeftClickAttack");
            Debug.LogFormat("클릭했음");
            

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
            else if(mousePosition.x < gameObject.transform.position.x)
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

            //playerAni.SetBool("LeftClickAttack", leftClickAttack);

            //this.gameObject.transform.position = 

            Debug.LogFormat("이것이 현재 마우스의 좌표? -> {0}", mousePosition);
        }
        
    }   // AttackClick()

}   // NameSpace
