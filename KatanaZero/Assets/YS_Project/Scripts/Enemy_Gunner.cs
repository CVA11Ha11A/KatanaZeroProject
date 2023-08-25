using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rewired.Data.ConfigVars;

public class Enemy_Gunner : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol
    }
    public EnemyState startingState = EnemyState.Patrol; // 시작 상태 설정
    public GameObject[] bloodPrefabs;

    private EnemyState currentState;
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
    public bool isDie = false;
    private bool isGrounded;
    private Rigidbody2D enemyRigid;
    AudioSource deathSound;
    CameraShake cameraShake;
    CapsuleCollider2D enemyCollider;
    public GameObject followMark;
    // Start is called before the first frame update
    void Start()
    {
        enemyCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        enemyRigid = GetComponent<Rigidbody2D>();
        cameraShake = FindAnyObjectByType<CameraShake>();
        currentState = startingState;
        deathSound = GetComponent<AudioSource>();
        if (currentState == EnemyState.Patrol)
        {
            SelecTarget();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            gun.SetActive(false);
            return;
        }

        if (inRange)
        {

            followMark.SetActive(true);
            anim.Play("Gangster_aim");
            gun.SetActive(true);

        }
        else
        {
            followMark.SetActive(false);

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
        if (!InsideofLisits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Gangster_aim"))
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
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Patrol:
                HandlePatrolState();
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            Physics2D.IgnoreCollision(collision.collider, enemyCollider);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isDie == false)
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
        if (target == null)
        {
            return;
        }
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
        int randomIdx = Random.Range(0, 3);
        GameObject blood = Instantiate(bloodPrefabs[randomIdx], transform.position, transform.rotation);
        deathSound.Play();
        cameraShake.ShakeCamera();
        anim.Play("Gangster_Die");
        EnemyCountManager.Instance.currentCount += 1;
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
        if(startingState==EnemyState.Patrol)
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
    }
    private bool InsideofLisits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }
    void HandleIdleState()
    {
        if (inRange)
        {
            anim.Play("Gangster_aim");
            gun.SetActive(true);
        }
        else
        {
            gun.SetActive(false);
        }

        if (!attackMode)
        {
            Move();
        }
    }

    void HandlePatrolState()
    {
        if (inRange)
        {
            currentState = EnemyState.Idle; // 플레이어를 만나면 공격 상태로 전환
            return;
        }

        Move(); // 계속해서 순찰 진행

        if (InsideofLisits())
        {
            SelecTarget();
        }
    }

}
