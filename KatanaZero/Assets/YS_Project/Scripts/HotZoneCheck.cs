using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private EnemyRay enemyParent;
    private bool inRange;
    private Animator anim;
    TimeBody timeBody;

    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyRay>();
        anim = GetComponentInParent<Animator>();
        timeBody = GetComponentInParent<TimeBody>();
    }

    private void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Grunt_attack"))
            {
            if(enemyParent.isDie==false)
            {
            enemyParent.Flip();

            }
        }
        if(timeBody.isRewindOver)
        {
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange = false;
            enemyParent.SelecTarget();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange = false;
            enemyParent.SelecTarget();
        }
    }
}
