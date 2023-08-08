using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chasing : MonoBehaviour
{
    public enum EnemyState
    {
        idle,lookleft, lookright,leftChasing, rightChasing
    }
    Rigidbody2D enemyRigid;
    EnemyState enemyState;
    private float moveSpeed = 3f;
    private bool isMoveLeft;
    private bool isMoveRight;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigid=GetComponent<Rigidbody2D>();
        enemyState=EnemyState.lookleft;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x == -1)
        {
            
            enemyState = EnemyState.lookleft;
           
        }
        if (transform.localScale.x == 1)
        {
           


            enemyState = EnemyState.lookright;
        }

        if (enemyState == EnemyState.lookleft)
        {
            isMoveLeft = true;
            isMoveRight = false;
        }
        if(enemyState == EnemyState.lookright)
        {
            isMoveLeft = false;
            isMoveRight = true;
        }
        if(enemyState == EnemyState.idle)
        {
            isMoveLeft = false;
            isMoveRight = false;
        }

        if (isMoveLeft)
        {
            Vector3 moveMent = new Vector3(-moveSpeed, enemyRigid.velocity.y, 0f);
            enemyRigid.velocity = moveMent;
        }
        if(isMoveRight)
        {
            Vector3 moveMent = new Vector3(moveSpeed, enemyRigid.velocity.y, 0f);
            enemyRigid.velocity = moveMent;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Turn")&&transform.localScale.x==-1)
        {
             Vector3 turnRight = new Vector3(1, 1, 1);
            transform.localScale = turnRight;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Turn")&&transform.localScale.x==-1)
        {
           
        }
    }
    public void TurnEnemy()
    {
        if(enemyState==EnemyState.lookleft)
        {
        Vector3 turnRight = new Vector3(1, 1, 1);
        transform.localScale = turnRight;
            enemyState = EnemyState.lookright;

        }
        if(enemyState == EnemyState.lookright)
        {
            Vector3 turnLeft = new Vector3(-1, 1, 1);
            transform.localScale = turnLeft;
            enemyState = EnemyState.lookleft;

        }
    }

}
