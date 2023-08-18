using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag.Equals("Bullet"))
    //    {
    //        // Calculate the direction from the bullet to the player
    //        Vector2 repelDirection = (transform.position - collision.transform.position).normalized;

    //        // Apply force to the bullet
    //        Rigidbody2D bulletRigidbody = collision.GetComponent<Rigidbody2D>();
    //        if (bulletRigidbody != null)
    //        {
    //            bulletRigidbody.velocity = Vector2.zero; // Reset any previous velocity
    //            bulletRigidbody.AddForce(repelDirection * 10f, ForceMode2D.Impulse);
    //        }
    //    }
    //}
}
