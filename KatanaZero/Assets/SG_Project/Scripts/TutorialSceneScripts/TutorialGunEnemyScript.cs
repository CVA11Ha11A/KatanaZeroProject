using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialGunEnemyScript : MonoBehaviour
{
    #region 이벤트 Action
    public event Action<bool> enemyShotEvent;
    // Start is called before the first frame update
    private bool isShot = false;

    public bool IsShot
    {
        get { return isShot; }

        set
        {
            if (isShot != value)
            {
                isShot = value;
                enemyShotEvent?.Invoke(isShot);
            }
        }
    }
    #endregion

    public GameObject bulletMaker;
    public GameObject bloodPrefab;
    Coroutine letShot;
    Coroutine bloodCorouTine;
    Coroutine enemyTextCoroutine;

    WaitForFixedUpdate waitForFixedUpdate = default;
    WaitForSeconds waitForSeconds = default;

    Rigidbody2D rigid;
    BoxCollider2D boxCollider;

    Animator animator;
    AudioSource audioSource;


    public GameObject meetEnemyText;
    public GameObject meetPlayerText;

    // [0] = 쓰러지는 소리    [1] = 총 맞았을때 날 소리
    [SerializeField] AudioClip[] audioClip;

    private GameObject bloodClone;

    //죽었는지
    bool isDead = false;

    // 만든적이 있는지
    bool oneMakeBlood = false;

    // 만드는중
    bool Bloodmaking = false;



    void Start()
    {
        if (waitForFixedUpdate == default || waitForFixedUpdate == null)
        {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }
        if (rigid == default || rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        if (boxCollider == default || boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        if (animator == default || animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (waitForSeconds == default || waitForSeconds == null)
        {
            waitForSeconds = new WaitForSeconds(0.1f);
        }

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == true && oneMakeBlood == false && Bloodmaking == false)
        {
            bloodCorouTine = StartCoroutine(MakeBlood());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.CompareTag("Player"))
        //{
        //    letShot = StartCoroutine(StartShot());
        //}


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            letShot = StartCoroutine(StartShot());
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyTextCoroutine = StartCoroutine(MeetEnemyTextStart());
        }

        if (collision.gameObject.CompareTag("SG_Bullet"))
        {
            meetPlayerText.gameObject.SetActive(false);

            // 동작은 뒤로 날아가는게 맞음
            if (this.gameObject.transform.position.x < collision.gameObject.transform.position.x)
            {                
                audioSource.clip = audioClip[1];
                audioSource.Play();

                isDead = true;
                // 뒤로 날아감
                rigid.AddForce(new Vector2(-13f, 5f), ForceMode2D.Impulse);
                rigid.gravityScale = 1f;
                boxCollider.isTrigger = false;
                animator.SetTrigger("EnemyDieTrigger");

            }
            else if (this.gameObject.transform.position.x > collision.gameObject.transform.position.x)
            {
                audioSource.clip = audioClip[1];
                audioSource.Play();

                isDead = true;
                // 앞로 날아감
                rigid.AddForce(new Vector2(13f, 5f), ForceMode2D.Impulse);
                rigid.gravityScale = 1f;
                boxCollider.isTrigger = false;
                animator.SetTrigger("EnemyDieTrigger");
            }
            if (isDead == true)
            {
                audioSource.clip = audioClip[0];
                audioSource.Play();
            }
        }
    }


    private IEnumerator StartShot()
    {
        meetPlayerText.gameObject.SetActive(true);
        //  waitForFixed는 5번 돌아야 0.1초
        for (int i = 0; i <= 53; i++)
        {
            yield return waitForFixedUpdate;
        }
        bulletMaker.gameObject.SetActive(true);

        IsShot = true;

    }

    private IEnumerator MakeBlood()
    {
        Bloodmaking = true;
        for (int i = 0; i <= 20; i++)
        {
            bloodClone = Instantiate(bloodPrefab, this.transform);
            yield return waitForSeconds;
        }
    }

    private IEnumerator MeetEnemyTextStart()
    {
        meetEnemyText.gameObject.SetActive(true);
        for (int i = 0; i <= 5; i++)
        {
            yield return waitForSeconds;
        }
        meetEnemyText.gameObject.SetActive(false);
    }

}
