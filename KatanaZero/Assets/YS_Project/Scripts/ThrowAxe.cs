using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAxe : MonoBehaviour
{
    public float speed = 10f;
    public AudioClip reflectSound;
    Rotate rotateClass;
    Rigidbody2D rb;
    private Vector3 rotate;
    private bool isWall=false;
    private bool isReturn = false;
    Kissyface_manager manager;
    AxeSoundEffect soundClass;
    // Start is called before the first frame update
    void Start()
    {
        soundClass = FindAnyObjectByType<AxeSoundEffect>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        rotateClass = GetComponentInChildren<Rotate>();
        manager = FindAnyObjectByType<Kissyface_manager>();
       

    }

    // Update is called once per frame
    void Update()
    {
       if(manager.isHit)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Wall"))
        {
            isWall = true;
            rb.velocity = Vector2.zero;
            rotateClass.isStop = true;
            StartCoroutine(Reverse());
        }
        if(collision.tag.Equals("Boss"))
        {

            Kissyface_Throw throwClass = collision.GetComponent<Kissyface_Throw>();
            if(throwClass!=null)
            {
                if(isWall==true)
                {
                StartCoroutine(throwClass.ReturnRoutine());
                    Destroy(gameObject);
                }
            }
        }
        if(collision.tag.Equals("PlayerAttack"))
        {
            Kissyface_Throw throwClass = FindAnyObjectByType<Kissyface_Throw>();
            if (throwClass != null)
            {
                soundClass.ReflectSound(reflectSound);
                isReturn = true;
                isWall = true;
                rb.velocity = Vector2.zero;
                FastReverse();
            }
        }
        if(collision.tag.Equals("Player"))
        {
            PlayerMove playerMove = collision.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                if(isReturn)
                {
                    return;
                }
                if (playerMove.isDodge == false&&playerMove.isDie==false)
                {
                    
                playerMove.Die();
                }

            }
        }
    }
    private IEnumerator Reverse()
    {
        yield return new WaitForSeconds(0.5f);
      
        rotateClass.isWall = true;
        rotateClass.isStop = false;
        rb.velocity = transform.right * -speed;

    }
    private void FastReverse()
    {
        rotateClass.isWall = true;
        rotateClass.isStop = false;
        rb.velocity = transform.right * (-speed*2);
    }
}
