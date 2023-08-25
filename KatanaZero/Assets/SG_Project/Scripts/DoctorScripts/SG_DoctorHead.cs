using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_DoctorHead : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.Play("RollHead");
        rigid.AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            animator.Play("FloorHead");
        }
    }
}
