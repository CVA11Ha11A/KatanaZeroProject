using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerAttack : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject slashEffect;
    float direction;
    Animator slashAni;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        slashAni = GetComponent<Animator>();
        slashEffect = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        transform.position = playerObj.transform.position;

        //slashEffect.SetActive(true);
        var dir = point - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        slashAni.Play("SlashEffect");
        Vector2 attackDirection = (point - playerObj.transform.position).normalized;
        //ApplyForceToEnemies(attackDirection);


    }
    public void DisableEffect()
    {
        gameObject.SetActive(false);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag.Equals("Enemy"))
        {
            // Calculate the direction from the enemy to the player
            Vector2 attackDirection = (playerObj.transform.position - collision.transform.position).normalized;
            collision.GetComponent<EnemyRay>().Die();

            // Apply force to the enemy
            Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.velocity = Vector2.zero; // Reset any previous velocity
                enemyRigidbody.AddForce(-attackDirection * 10f, ForceMode2D.Impulse);
            }
        }
        if(collision.tag.Equals("Breakable"))
        {
            collision.GetComponent<BreakableObject>().BreakPlatform();
        }
    }
    
}

