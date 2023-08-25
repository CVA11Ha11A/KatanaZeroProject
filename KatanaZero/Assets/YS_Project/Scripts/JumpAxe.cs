using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAxe : MonoBehaviour
{
    public Transform center;    // 원운동 중심점
    public float radius; // 반지름
    public float speed = 20f;  // 속도
    bool isMaxSize = false;
    private Vector3 initialCenterPosition; // 초기 중심점 위치
    private float angle = 0;
    Kissyface_manager manager;
    private void Start()
    {
        radius = 0.1f;
        Destroy(gameObject, 0.8f);
        initialCenterPosition = transform.position;
        manager = FindAnyObjectByType<Kissyface_manager>();
    }
    void Update()
    {
        if(manager.isHit)
        {
            Destroy(gameObject);
        }
        if(radius<0)
        {
            return;
        }
        if (radius >=3f)
        {
            isMaxSize = true;

        }
        
        if(!isMaxSize)
        {
            radius += 7f*Time.deltaTime;

        }
        else
        {
            radius -= 7f * Time.deltaTime;

        }
        angle += speed * Time.deltaTime;
        //transform.position = center.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        Vector3 newPosition = initialCenterPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        transform.position = newPosition;
        Vector3 lookDirection = initialCenterPosition - newPosition;
        float rotationAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle+90f);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            PlayerMove playerMove = collision.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                if(playerMove.isDodge==false&&playerMove.isDie==false)
                {
                playerMove.Die();

                }
            }
        }
    }
}
