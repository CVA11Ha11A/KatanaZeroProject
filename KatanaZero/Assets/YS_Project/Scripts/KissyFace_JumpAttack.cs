using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KissyFace_JumpAttack : MonoBehaviour
{
    public GameObject attackPrefab;
    private float axeTimer = 0;
    private float axeRate = 1f;
    Animator kissyAni;
    Rigidbody2D rb;
    bool jumpAttack = false;
    Kissyface_manager manager;
    private IEnumerator jumpAttackCoroutine;
    private IEnumerator axeCoroutine;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        manager = GetComponent<Kissyface_manager>();
        kissyAni = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        jumpAttack = false;
        manager.isAttackable = true;
        jumpAttackCoroutine = JumpAttack();
        StartCoroutine(jumpAttackCoroutine);

    }

    // Update is called once per frame
    void Update()
    {
        if(manager.isHit)
        {
            StopCoroutine(jumpAttackCoroutine);
            StopCoroutine(axeCoroutine);
            return;
        }
        
        
    }
    private IEnumerator AxeRoutine()
    {
        Debug.Log("AxeRoutine() 실행1");

        yield return new WaitForSeconds(0.2f);
        GameObject axe = Instantiate(attackPrefab, transform.position, transform.rotation);
       
        rb.gravityScale = 1.2f;
        yield return new WaitForSeconds(1f);
        if (manager.isHit)
        {
            yield break;

        }
        Debug.Log("AxeRoutine() 실행2");
        
        kissyAni.Play("Kissyface_landattack");
        manager.isAction = false;

    }
    private IEnumerator JumpAttack()
    {
            kissyAni.Play("Kissyface_prejump");
        yield return new WaitForSeconds(0.5f);
        if (!jumpAttack)
        {
        kissyAni.Play("Kissyface_jump");

            jumpAttack = true;
            Vector3 jump = new Vector3(0, 7f, 0);
            rb.AddForce(jump, ForceMode2D.Impulse);
            axeCoroutine= AxeRoutine();
            if (manager.isHit)
            {
                StopCoroutine(jumpAttackCoroutine);
                StopCoroutine(axeCoroutine);
               
            }
            Debug.Log("AxeRoutine() 실행");

            StartCoroutine(axeCoroutine);
        }
    }

}
