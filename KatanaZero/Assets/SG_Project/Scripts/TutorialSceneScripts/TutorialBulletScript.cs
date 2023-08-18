using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;

public class TutorialBulletScript : MonoBehaviour
{
    public GameObject enemy;

    private Rigidbody2D rigid;
    private BoxCollider2D boxCollider;

    bool front = false;
    bool back = false;

    bool reflex = false;
    bool reflexFlont = false;
    bool reflexBack = false;

    float bulletSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (front == true && reflex == false)
        {
            transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
        }
        else if (back == true && reflex == false)
        {
            transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime);
        }
        else { /*PASS*/ }

        if(reflex == true && reflexFlont == true)
        {
            transform.Translate(Vector3.right * (bulletSpeed * 2) * Time.deltaTime);
        }
        else if(reflex == true && reflexBack == true)
        {
            transform.Translate(Vector3.left * (bulletSpeed * 2) * Time.deltaTime);
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("SG_PlayerAttack"))
        {
            reflex = true;
            if(collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
            {
                reflexBack = true;
            }
            else if(collision.gameObject.transform.position.x < this.gameObject.transform.position.x)
            {
                reflexFlont = true;
            }
        }
    }

    public void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        if(enemy.transform.position.x < this.transform.position.x)
        {
            front = true;
        }
        else if(enemy.transform.position.x> this.transform.position.x)
        {
            back = true;    
        }
    }
}
