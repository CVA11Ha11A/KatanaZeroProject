using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rewired.Data.ConfigVars;

public class Enemy_Gunner : MonoBehaviour
{
    public float attackDistance;
    public float moveSpeed;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public Transform leftLimit;
    public Transform rightLimit;
    public GameObject hotZone;
    public GameObject triggerArea;
    public GameObject gun;
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool cooldown;
    private bool onPlatform;
    private bool onStair;
    private bool isDie = false;
    private bool isGrounded;
    private Rigidbody2D enemyRigid;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyRigid = GetComponent<Rigidbody2D>();
        SelecTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDie)
        {
            gun.SetActive(false);
            return; 
        }

        if (inRange)
        {
           

            anim.Play("Gangster_aim");
            gun.SetActive(true);
           
        }
        else
        {
            gun.SetActive(false);
        }
        if (isDie)
        {
            return;
        }
        if (!attackMode)
        {
            Move();

        }
        if (!InsideofLisits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Grunt_attack"))
        {
            SelecTarget();
        }
        if (inRange)
        {
            enemyRigid.velocity = Vector3.zero;
            //if (onPlatform && target.position.y < transform.position.y - 0.5f)
            //{
            //    platformPass.isPass = true;
            //}
            //else
            //{
            //    platformPass.isPass = false;

            //}
            if (target.position.y > transform.position.y + 0.5f && onStair == true)
            {
                target = leftLimit;

            }

            anim.SetBool("Run", true);
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")&&isDie==false)
            {
                inRange = true;
            }


        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Wall")
            {
                return;
            }
            if (collision.gameObject.tag == "Player")
            {
                if (isDie)
                {
                    target = null;
                    return;
                }
                if (!onStair)
                {
                    target = collision.transform;

                }
                inRange = true;
                //Debug.Log("player가 트리거에 들어옴");
            }
            Flip();
        }
        void Move()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Gangster_aim") && target.gameObject.tag != "Player")
            {
                anim.SetBool("Run", false);

                anim.SetBool("Walk", true);
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
                //transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                enemyRigid.velocity = new Vector2(moveDirection.x * moveSpeed, enemyRigid.velocity.y);
            }
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Gangster_aim") && target.gameObject.tag == "Player")
            {
                anim.SetBool("Walk", false);

                anim.SetBool("Run", true);
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
                //transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                enemyRigid.velocity = new Vector2(moveDirection.x * (moveSpeed * 2), enemyRigid.velocity.y);
            }

        }
        public void Die()
        {
        anim.Play("Gangster_Die");
        isDie = true;
        }
        public void Flip()
        {
            if (target == null)
            {
                return;
            }
            Vector3 scale = transform.localScale;
            if (transform.position.x > target.position.x)
            {
                scale.x = -1;
            }
            else
            {
                scale.x = 1;
            }
            transform.localScale = scale;
        }
        public void SelecTarget()
        {
            float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
            float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);
            if (distanceToLeft > distanceToRight)
            {
                target = leftLimit;
            }
            else
            {
                target = rightLimit;
            }
            Flip();
        }
    private bool InsideofLisits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

}
