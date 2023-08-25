using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;
using Unity.VisualScripting;

public class Kissyface_manager : MonoBehaviour
{
    public GameObject headPrefab;
    public GameObject playerObj;
    public GameObject sliderObj;
    public AudioClip hitClip;
    public AudioClip dieClip;
    public AudioClip guardClip;

    public bool isAction = false;
    public int pattern = 0;
    public int lastPattern = 0;
    public CapsuleCollider2D playerCollider;
    public Transform playerTransform;
    public bool isAttackable = false;
    public bool isHit = false;
    public Slider gageSlider;
    private float maxGage = 100;
    public float fillSpeed = 0.1133f; // 11.33...% / 초
    private float currentFill = 0f;
    private float struggleTimer = 0;
    private float struggleRate = 3;

    private bool isHeadSpawn = false;
    public bool isDie = false;
    private bool isStruggle = false;
    private bool isBlock = false;
    private KissyFace_JumpAttack jumpAttack;
    private Kissyface_Lunge lunge;
    private Kissyface_Throw throwAttack;
    private BoxCollider2D kissyfaceCollider;
    
    const int JUMP_ATTACK = 1;
    const int LUNGE = 2;
    const int THROW = 3;
    Animator anim;
    float distance;
    AudioSource audioSource;
    Player player;
    TimeBody timeBody;
    Vector3 leftAngle = new Vector3(0, 180, 0);
    Vector3 rightAngle = new Vector3(0, 0, 0);
    Vector3 playerAttackPosition;
    Vector3 jailPosition = new Vector3(-20, -3.8f, 0);
    Rigidbody2D rb;
    CameraShake cameraShake;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timeBody = GetComponent<TimeBody>();
        gageSlider.value = 0;
        player = ReInput.players.GetPlayer(0);
        cameraShake = FindAnyObjectByType<CameraShake>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        kissyfaceCollider = GetComponent<BoxCollider2D>();

        throwAttack = GetComponent<Kissyface_Throw>();
        jumpAttack = GetComponent<KissyFace_JumpAttack>();
        lunge = GetComponent<Kissyface_Lunge>();

        StartCoroutine(SelectAction());
        Physics2D.IgnoreCollision(kissyfaceCollider, playerCollider);

    }

    // Update is called once per frame
    void Update()
    {
        if (timeBody.isRewindin)
        {
            isAction = true;
            StopAllCoroutines();
            return;
        }
        distance = Vector2.Distance(playerTransform.position, transform.position);

        if (isStruggle)
        {

            if (player.GetButton("Attack"))
            {
                
                sliderObj.SetActive(true);
                struggleTimer += Time.deltaTime;
                if (struggleTimer >= struggleRate)
                {
                    isStruggle = false;

                    struggleTimer = 0;
                    sliderObj.SetActive(false);
                    StartCoroutine(PlayerPositionReset());
                    StartCoroutine(RecoverRoutine());
                }
                currentFill += fillSpeed * Time.deltaTime;
                gageSlider.value = currentFill;
            }
        }

        //Debug.Log(currentFill);
        if (currentFill >= 1)
        {
            if (isDie == false)
            {

                StopAllCoroutines();
                audioSource.clip = dieClip;
                audioSource.Play();
                anim.Play("Kissyface_die");
                isDie = true;
                StartCoroutine(DieRoutine());

            }
            return;
        }
        if (!isAction)
        {

            rb.gravityScale = 1;
            isAttackable = false;
            if (playerTransform.position.x < transform.position.x && transform.eulerAngles != leftAngle)
            {
                transform.eulerAngles = leftAngle;
            }
            else if (playerTransform.position.x > transform.position.x && transform.eulerAngles != rightAngle)
            {
                transform.eulerAngles = rightAngle;
            }
            lunge.enabled = false;
            jumpAttack.enabled = false;
            throwAttack.enabled = false;
            StartCoroutine(SelectAction());
        }
        if (isHit)
        {
            lunge.enabled = false;
            jumpAttack.enabled = false;
            throwAttack.enabled = false;
            isAction = true;
        }
        if (!isHit && isBlock)
        {
            lunge.enabled = false;
            jumpAttack.enabled = false;
            throwAttack.enabled = false;
            isAction = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            
            Debug.Log("공격에 맞았다.");
            if (isDie)
            {
                anim.Play("Kissyface_nohead");
                if (isHeadSpawn == false)
                {
                    isHeadSpawn = true;
                    GameObject head = Instantiate(headPrefab, transform.position, transform.rotation);
                }
                return;
            }
            if (isAttackable)
            {
                cameraShake.ShakeCamera();
                if (isHit && isDie == false)
                {
                    StopAllCoroutines();
                    anim.Play("Kissyface_struggle");
                    playerAttackPosition = playerObj.transform.position;
                    playerObj.transform.position = jailPosition;
                    isStruggle = true;
                    isAction = true;
                }
                if (!isHit && isDie == false)
                {
                    StartCoroutine(RecoverRoutine());

                }
                anim.Play("Kissyface_hurt");
                isHit = true;
                isAction = true;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 2;


            }
            else
            {
                anim.Play("Kissyface_block");
                isAction = true;
                isBlock = true;
                StartCoroutine(BlockRoutine());
            }

        }
    }
    private IEnumerator SelectAction()
    {

        float waitSeconds;
        if (!isAction && !isBlock)
        {
            isAction = true;
            if (lastPattern == LUNGE)
            {
                waitSeconds = 1f;
            }
            else
            {
                waitSeconds = 0.5f;
            }
            while (lastPattern == pattern)
            {
                pattern = Random.Range(1, 4);

            }
            if(distance<=1.8f&&pattern==LUNGE)
            {
                while(pattern==LUNGE)
                {
                    pattern = Random.Range(1, 4);

                }
            }

            lastPattern = pattern;

            yield return new WaitForSeconds(waitSeconds);
            if (isBlock)
            {
                yield break;
            }
            if (pattern == JUMP_ATTACK)
            {
                jumpAttack.enabled = true;
            }
            else if (pattern == LUNGE)
            {
                lunge.enabled = true;
            }
            else if (pattern == THROW)
            {
                throwAttack.enabled = true;
            }
        }
    }
    private IEnumerator RecoverRoutine()
    {
        yield return new WaitForSeconds(1);
        if (isDie)
        { yield break; }
        anim.Play("Kissyface_recover");
        yield return new WaitForSeconds(0.5f);

        isAction = false;
        isHit = false;

    }
    private IEnumerator BlockRoutine()
    {
        audioSource.clip = guardClip;
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        StopAllCoroutines();
        isAction = false;
        isBlock = false;
    }
    private IEnumerator PlayerPositionReset()
    {
        yield return new WaitForSeconds(1);
        playerObj.transform.position = playerAttackPosition;

    }
    private IEnumerator DieRoutine()
    {


        StartCoroutine(PlayerPositionReset());
        yield return new WaitForSeconds(1f);

        anim.Play("Kissyface_dead");

    }
}
