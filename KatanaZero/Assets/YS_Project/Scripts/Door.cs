using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorAttackPrefab;
    BoxCollider2D doorCollider;
    Animator doorAni;
    // Start is called before the first frame update
    void Start()
    {
        doorCollider=GetComponent<BoxCollider2D>();
        doorAni=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(doorAni.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen")&&doorAni.GetCurrentAnimatorStateInfo(0).normalizedTime>=1.0f)
        {
            doorCollider.enabled=false;
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            
            doorAni.SetTrigger("DoorOpen");
            Instantiate(doorAttackPrefab, transform.position, transform.rotation);
        }
    }
}
