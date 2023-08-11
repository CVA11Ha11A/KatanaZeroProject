using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerMove;

public class Enemy_Normal : MonoBehaviour
{
    public enum EnemyType
    {
        stand, moveAround
    }
    public EnemyType type;
    public bool isPlayerIn = false;
    private float moveSpeed = 3f;
    private WaitForSeconds missingPlayer = new WaitForSeconds(5f);
    private bool isGoLeft;
    private bool isGoRight;
    private bool isDie = false;
    private bool isGround;
    private bool isChasing=false;

    BoxCollider2D enemyBoxCollider;
    Animator enemyAni;
    public GameObject target;
    Rigidbody2D enemyRigid;
    Vector3 leftScale = new Vector3(-1, 1, 1);
    Vector3 rightScale = new Vector3(1, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
        enemyAni = GetComponent<Animator>();
        enemyRigid = GetComponent<Rigidbody2D>();
        enemyBoxCollider = GetComponent<BoxCollider2D>();
        if (type == EnemyType.moveAround)
        {
            isGoLeft = true;
            StartCoroutine(MoveAround());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Physics2D.Raycast(wallCheck.position, Vector2.right * playerScale, wallCheckDis, wall_mask);
        if (isDie==true&&isGround)
        {
            enemyRigid.velocity = Vector3.zero;
            enemyRigid.gravityScale = 0;
            enemyBoxCollider.enabled = false;
        }
        if (isDie)
        {
            return;
        }
       // Debug.LogFormat("isplayerIn [{0}]", isPlayerIn);
        if (isPlayerIn == true)
        {
            StopCoroutine(MoveAround());
            Vector3 playerPosition = target.transform.position;
            if (transform.position.x > playerPosition.x)
            {
                Vector3 moveLeft = new Vector3(-moveSpeed - 2, 0, 0);
                transform.localScale = leftScale;
                enemyRigid.velocity = moveLeft;
            }
            if (transform.position.x < playerPosition.x)
            {
                Vector3 moveRight = new Vector3(moveSpeed + 2, 0, 0);
                transform.localScale = rightScale;

                enemyRigid.velocity = moveRight;
            }
            isChasing = true;
            return;
        }
        else
        {
            if (type == EnemyType.moveAround)
            {
                if (isGoLeft)
                {
                    Vector3 moveLeft = new Vector3(-moveSpeed, 0, 0);
                    enemyRigid.velocity = moveLeft;
                }
                if (isGoRight)
                {
                    Vector3 moveRight = new Vector3(moveSpeed, 0, 0);
                    enemyRigid.velocity = moveRight;
                }
            }
        }
        if(isChasing)
        {

        }
    }

    public void Die()
    {
        enemyAni.SetTrigger("Die");
        isDie = true;
        StopCoroutine(MoveAround());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Floor"))
        {
            isGround = true;
        }
        if(collision.collider.tag.Equals("Platform"))
        {
            isGround = true;

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Floor"))
        {
            isGround = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            isPlayerIn = true;
            target = collision.gameObject;
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{

    //    StartCoroutine(PlayerMissing());
    //}
    //private IEnumerator PlayerMissing()
    //{
    //    yield return missingPlayer;
    //    isPlayerIn = false;
    //}

    //private IEnumerator MoveAround()
    //{
    //    if(isGoLeft)
    //    {
    //        isGoLeft = false;
    //        isGoRight = true;

    //    }
    //    else if(isGoRight)
    //    {
    //        isGoLeft = true;
    //        isGoRight = false;
    //    }
    //        yield return new WaitForSeconds(5f);
    //    if (isGoLeft)
    //    {
    //        isGoLeft = false;
    //        isGoRight = true;

    //    }
    //    else if (isGoRight)
    //    {
    //        isGoLeft = true;
    //        isGoRight = false;
    //    }
    //}
    private IEnumerator MoveAround()
    {
        while (true)
        {
            if(isDie)
            {
                break;
            }
            if (isPlayerIn)
            {
                break;
            }
            if (isGoLeft)
            {
                Vector3 moveLeft = new Vector3(-moveSpeed, 0, 0);
                enemyRigid.velocity = moveLeft;
                transform.localScale = leftScale;
            }
            else if (isGoRight)
            {
                Vector3 moveRight = new Vector3(moveSpeed, 0, 0);
                enemyRigid.velocity = moveRight;
                transform.localScale = rightScale;

            }

            yield return new WaitForSeconds(3f);

            
            isGoLeft = !isGoLeft;
            isGoRight = !isGoRight;
        }
    }
}
