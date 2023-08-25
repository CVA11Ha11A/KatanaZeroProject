using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerAttack : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject slashEffect;
    public AudioClip[] slashClip;
    public AudioClip killClip;
    float direction;
    Animator slashAni;
    Player player;
    AudioSource slashSound;
    // Start is called before the first frame update
    private void OnEnable()
    {
        slashSound = GetComponent<AudioSource>();
        int randomIdx = Random.Range(0, 4);
        slashSound.clip = slashClip[randomIdx];
        slashSound.Play();
    }
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
            EnemyRay enemyCollision = collision.GetComponent<EnemyRay>();
            if(enemyCollision!=null)
            {
                slashSound.clip = killClip;
                slashSound.Play();
                if(enemyCollision.isDie==false)
                {
                enemyCollision.Die();
                    Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();
                    if (enemyRigidbody != null)
                    {
                        enemyRigidbody.velocity = Vector2.zero; // Reset any previous velocity
                        enemyRigidbody.AddForce(-attackDirection * 10f, ForceMode2D.Impulse);
                    }
                }
            }
            Enemy_Gunner gunnerCollision = collision.GetComponent<Enemy_Gunner>();
            if(gunnerCollision!=null)
            {
                slashSound.clip = killClip;
                slashSound.Play();

                if(gunnerCollision.isDie==false)
                {
                gunnerCollision.Die();
                    Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();
                    if (enemyRigidbody != null)
                    {
                        enemyRigidbody.velocity = Vector2.zero; // Reset any previous velocity
                        enemyRigidbody.AddForce(-attackDirection * 10f, ForceMode2D.Impulse);
                    }
                }
            }
           

            // Apply force to the enemy
            
        }
        if(collision.tag.Equals("Breakable"))
        {
            collision.GetComponent<BreakableObject>().BreakPlatform();
        }
        if(collision.tag.Equals("Door"))
        {
            Door openDoor = collision.GetComponent<Door>();
            Debug.Log("Door에 공격");
            if(openDoor!=null)
            {
                Debug.Log("Door가 null이 아니다");

                openDoor.DoorOpen();
            }
        }
    }
    
}

