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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;

        initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall") || collision.tag.Equals("Floor"))
        {
            Destroy(gameObject);
        }
        else if (collision.tag.Equals("Reflect") && !isReflected)
        {
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
    }

    private void ReflectBullet()
    {
        Vector2 normal = new Vector2(-rb.velocity.x, -rb.velocity.y).normalized;  // 현재 이동 방향의 반대 벡터 계산
        Vector2 reflectedDirection = Vector2.Reflect(rb.velocity.normalized, normal);  // 법선에 대한 반사 방향 계산
        rb.velocity = reflectedDirection * speed;
        isReflected = true;
    }
}
