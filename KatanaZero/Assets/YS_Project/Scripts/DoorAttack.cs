using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Enemy"))
        {
            collision.GetComponent<Enemy_Normal>().Die();
            //Destroy(gameObject);
        }
    }
}
