using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerDieScript : MonoBehaviour
{
    // 플레이어가 죽었는지 살았는지 구별할 변수 다른곳에서 참조를위해 public 으로 열어둠
    public bool isPlayerDie = false;

    private Rigidbody2D rigid;

    public Sprite[] dieSprite = new Sprite[2];
    private SpriteRenderer spriteRenderer;
    private Animator playerAni;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isPlayerDie = true;
            playerAni.enabled = false;
            WhenPlayerDie();

            spriteRenderer.sprite = dieSprite[0];
            Invoke("DieRenderer", 0.5f);
            // 플레이어 날리기
            if (collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                rigid.AddForce(new Vector2(-8f, -5f), ForceMode2D.Impulse);
            }
            else if (collision.gameObject.transform.position.x < this.gameObject.transform.position.x)
            {
                rigid.AddForce(new Vector2(8f, 5f), ForceMode2D.Impulse);
            }

            // 0.5 초뒤 RigidBody상호작용 끄기
            Invoke("StopRigid", 0.5f);


        }
    }

    public void StopRigid()
    {
        rigid.simulated = false;
    }
    public void DieRenderer()
    {
        spriteRenderer.sprite = dieSprite[1];
    }


    public void WhenPlayerDie()
    {
        // 움직임 끄기
        this.gameObject.GetComponent<SG_PlayerMovement>().enabled = false;
    }
}
