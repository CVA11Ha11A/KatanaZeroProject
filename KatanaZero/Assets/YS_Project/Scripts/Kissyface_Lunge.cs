using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Kissyface_Lunge : MonoBehaviour
{
    public Transform target;
    public float lungeSpeed = 5f;
    public float stoppingDistance = 1.5f; // ¿øÇÏ´Â ¸ØÃã °Å¸® ¼³Á¤
    bool isjump = false;
    bool isAttack = false;
    bool isGrounded = false;
    bool isClose = false;
    float jumpForce;
    Animator anim;
    Rigidbody2D rb;
    Vector2 targetPosition;
    Vector2 initPosition;
    float initDistance;
    float distance;
    Kissyface_manager manager;
    CameraShake cameraShake;
    Vector3 leftAngle = new Vector3(0, 180, 0);
    Vector3 rightAngle = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>();
    }
    private void OnEnable()
    {
       
        manager = GetComponent<Kissyface_manager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        isjump = false;
        isClose = false;
        isAttack = false;
        manager.isAttackable = true;
        initPosition = transform.position;
        targetPosition = target.position;
        initDistance = Vector2.Distance(targetPosition, transform.position);
        anim.Play("Kissyface_prelunge");
        StartCoroutine(LungeRoutine());

    }
    // Update is called once per frame
    void Update()
    {

        //Debug.Log(distance);
        distance = Vector2.Distance(target.position, transform.position);
        
        if (distance < 1.8f&& isAttack)
        {
            // ´ë»ó°úÀÇ °Å¸®°¡ ¸ØÃã °Å¸® ÀÌÇÏ¸é ¸ØÃã
            rb.gravityScale = 2f;
            Vector2 stopx = new Vector2(0, rb.velocity.y);
            rb.velocity = stopx;
            if (!isClose)
            {
                cameraShake.ShakeCamera();
                 anim.Play("Kissyface_lungeAttack");
                isClose = true;
                manager.isAction = false;

            }

            
            return;
        }

       
        if (initDistance / 2 == distance)
        {
            rb.gravityScale = 2f;
        }

    }
    private IEnumerator LungeRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        anim.Play("Kissyface_lunge");
        if (!isjump)
        {
            isjump = true;
            
            if(distance<4)
            {
                jumpForce = 2.5f;
            }
            else
            {
                jumpForce = 5f;
            }
            Vector2 jump = new Vector2(0, jumpForce);
            rb.AddForce(jump, ForceMode2D.Impulse);

        }
        
            if (!isAttack&& distance > 1.8f)
            {
                isAttack = true;
                //Vector2 direction = (targetPosition - initPosition).normalized;
                //rb.AddForce(direction * (lungeSpeed * 2), ForceMode2D.Impulse);
            Vector2 direction = (targetPosition - initPosition).normalized;
            Vector2 force = new Vector2(direction.x * (lungeSpeed * 3), 0); // y Ãà ÈûÀ» 0À¸·Î ¼³Á¤
            rb.AddForce(force, ForceMode2D.Impulse);
        }
       

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
   
}
