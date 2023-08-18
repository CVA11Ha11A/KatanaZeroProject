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
            EnemyRay enemyGrunt = collision.GetComponent<EnemyRay>();
            if(enemyGrunt!=null)
            {
                enemyGrunt.Die();
            }
            Enemy_Gunner enemyGunner = collision.GetComponent<Enemy_Gunner>();
                if(enemyGunner!=null)
            {
                enemyGunner.Die();
            }
            
            //Destroy(gameObject);
        }
    }
}
