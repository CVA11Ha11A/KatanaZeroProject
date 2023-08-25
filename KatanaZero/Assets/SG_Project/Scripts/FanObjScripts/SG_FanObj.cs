using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_FanObj : MonoBehaviour
{
    SpriteRenderer fanSprite;
    BoxCollider2D fanCollider;
    TimeManager timeManager;
    bool isDamage;
    bool dodgeReturn;

    // Start is called before the first frame update
    void Start()
    {
        fanSprite = GetComponent<SpriteRenderer>();
        fanCollider = GetComponent<BoxCollider2D>();
        timeManager = FindAnyObjectByType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ColliderControler();
        //Debug.LogFormat(fanSprite.sprite.name);
        //spr_fanblade_0 ~ 31
    }

    public void ColliderControler()
    {
        if (fanSprite.sprite.name == "spr_fanblade_10")
        {
            //fanCollider.enabled = false;
            isDamage = false;
            
            fanSprite.color = Color.white;

        }
        else { /*PASS*/ }

        if (fanSprite.sprite.name == "spr_fanblade_21")
        {
            isDamage = true;
            //fanCollider.enabled = true;
            if(timeManager.isTimeSlow)
            {
            fanSprite.color = Color.red;

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            PlayerMove playerMove = collision.GetComponent<PlayerMove>();
            Rigidbody2D playerRigid = collision.GetComponent<Rigidbody2D>();
            if(playerMove!=null)
            {
                if (playerMove.isDodge && timeManager.isTimeSlow == false)
                {
                    
                   
                    playerRigid.velocity = Vector2.zero;
                    Vector2 blocking = new Vector2(-15,10);
                    playerRigid.AddForce(blocking, ForceMode2D.Impulse);

                    


                }
                else if(playerMove.isDodge==false)
                {
                    if (playerMove.isDie == false)
                    {

                    playerMove.Die();
                    }

                }
                else if(playerMove.isDodge&&timeManager.isTimeSlow&&isDamage)
                {
                 
                    playerRigid.velocity = Vector2.zero;
                    Vector2 blocking = new Vector2(-15, 10);
                    playerRigid.AddForce(blocking, ForceMode2D.Impulse);
                }
            }
        }
    }
   
}
