using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_HandEnemy : MonoBehaviour
{

    // Ray로 플레이어 확인하고 Collider로 피격 공격 판정 할 예정
   


    // 플레이어가 죽었는지 확인하기 위해 선언
    SG_PlayerMovement playerMovementClass;

    Animator animator;


    // 플레이어 발견했는지
    private bool discoverPlayer = false;    

    private bool handEnemyDie = false;



    // Start is called before the first frame update
    void Start()
    {
        playerMovementClass = FindAnyObjectByType<SG_PlayerMovement>();
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("SG_PlayerAttack"))
        {
            handEnemyDie = true;
            animator.SetTrigger("IsDieTrigger");
        }

    }

    // 플레이어를 감지 못했을떄에 움직임
    public void NonDiscoverPlayerMove()
    {
        if(discoverPlayer == false)
        {
            //animator.SetTrigger()
        }
    }
}
