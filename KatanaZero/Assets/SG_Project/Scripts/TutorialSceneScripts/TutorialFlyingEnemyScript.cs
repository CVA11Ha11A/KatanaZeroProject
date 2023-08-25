using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialFlyingEnemyScript : MonoBehaviour
{
    public GameObject bloodPrefab;
    private GameObject bloodClone;


    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClip;

    Coroutine enemyStart;
    WaitForSeconds waitForSecond;

    Rigidbody2D rigid;
    BoxCollider2D boxCollider;

    Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        waitForSecond = new WaitForSeconds(0.25f);
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            audioSource.clip = audioClip[1];
            audioSource.Play();
            animator.SetTrigger("DropDieTrigger");
            Destroy(rigid);
            boxCollider.isTrigger = true;          
        }

    }

    public void OnEnable()
    {
        // ON

        enemyStart = StartCoroutine(StartEnemy());
    }

    public void OnDisable()
    {
        // Off 

    }


    private IEnumerator StartEnemy()
    {
        
        for(int i =0; i <= 16; i++)
        {
            yield return waitForSecond;
        }
        audioSource.clip = audioClip[0];
        audioSource.Play();
        this.rigid.AddForce(new Vector2(6f, 2f),ForceMode2D.Impulse);


        for(int j = 0; j <= 10; j++)
        {
            bloodClone = Instantiate(bloodPrefab,this.transform);
            yield return waitForSecond;
        }

    }
}
