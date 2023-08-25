using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform target;
    public Transform parent;

    public Animator gunsparkAni;
    public GameObject gunspark;
    public GameObject bulletPrefab;
    public float maxAngle = 50f; // 최대 각도
    public float rotationSpeed = 5f; // 회전 속도
    public float moveSpeed = 3f; // 이동 속도
    private float shootingTimer=0;
    private float shootingRate = 0.7f;
    private AudioSource gunSound;
    PlayerMove playerMove;
    private void Start()
    {
        playerMove = FindAnyObjectByType<PlayerMove>();
        gunSound = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(playerMove.isDie==true)
        {
            gunsparkAni.enabled = false;
            return;
        }
        if (target == null)
            return;
        Vector3 point =target.transform.position;

        if(parent.localScale.x<0)
        {
            Vector3 scale = new Vector3(-1, -1, 1);
            transform.localScale = scale;
        }
        else
        {
            Vector3 originScale = new Vector3(1, 1, 1);
            transform.localScale = originScale;
        }
        //slashEffect.SetActive(true);
        var dir = point - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
       // slashAni.Play("SlashEffect");
        Vector2 attackDirection = (point - transform.position).normalized;
        //ApplyForceToEnemies(attackDirection);
       
            shootingTimer += Time.deltaTime;
        
        if(shootingTimer>=shootingRate)
        {
            gunSound.Play();
            gunspark.SetActive(true);
            // Calculate the bullet spawn position offset
            Vector3 bulletOffset = transform.right * 1f;
            Vector3 bulletPosition = transform.position + bulletOffset;
            bulletOffset.y = bulletOffset.y - 0.2f;
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition, transform.rotation);
            shootingTimer = 0f;
          

        }
        else
        {
            gunspark.SetActive(false);
        }
       
    }
}