using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner_hotzone : MonoBehaviour
{
    private Enemy_Gunner gunnerParent;
    private bool inRange;
    private Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        gunnerParent = GetComponentInParent<Enemy_Gunner>();
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Gangster_aim"))
        {
            gunnerParent.Flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            gunnerParent.triggerArea.SetActive(true);
            gunnerParent.inRange = false;
            //gunnerParent.SelecTarget();
        }
    }
}
