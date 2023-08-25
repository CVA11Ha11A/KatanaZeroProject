    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;
    Vector3 initialPosition;
    bool isReflected = false;
    public bool dieTrigger = false;
    TimeManager timeManager;
    TrailRenderer trail;
    AudioSource reflectSound;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        timeManager = FindAnyObjectByType<TimeManager>();
        trail = GetComponent<TrailRenderer>();
        initialPosition = transform.position;
        reflectSound = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (timeManager != null)
        {

        if(timeManager.isTimeSlow)
        {
                if(trail!=null)
                {
            trail.enabled = false;

                }
        }
        else
        {
                if (trail != null)
                {
                    trail.enabled = true;

                }
        }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall") || collision.tag.Equals("Floor") || collision.tag.Equals("Door") || collision.tag.Equals("Stair"))
        {
            Destroy(gameObject);
        }
        else if (collision.tag.Equals("Reflect") && !isReflected)
        {
            if(reflectSound!=null)
            {
            reflectSound.Play();

            }
            dieTrigger = true;
            ReflectBullet();
        }
        if(collision.tag.Equals("Enemy")&&dieTrigger==true)
        {
            EnemyRay enemyRay = collision.GetComponent<EnemyRay>();
            if (enemyRay != null)
            {
                enemyRay.Die();
            }

            Enemy_Gunner enemyGunner = collision.GetComponent<Enemy_Gunner>();
            if (enemyGunner != null)
            {
                enemyGunner.Die();
            }
            
        }
        if(collision.tag.Equals("Player"))
        {
            PlayerMove playerMove = collision.GetComponent<PlayerMove>();
            if(playerMove!=null)
            {
                if(playerMove.isDodge==false)
                {

                playerMove.Die();
                }
            }
        }
    }

    private void ReflectBullet()
    {
        Vector2 normal = new Vector2(-rb.velocity.x, -rb.velocity.y).normalized;  // 현재 이동 방향의 반대 벡터 계산
        Vector2 reflectedDirection = Vector2.Reflect(rb.velocity.normalized, normal);  // 법선에 대한 반사 방향 계산
        rb.velocity = reflectedDirection * speed;
        isReflected = true;
    }
}
