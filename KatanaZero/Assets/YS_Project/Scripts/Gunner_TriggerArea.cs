using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner_TriggerArea : MonoBehaviour
{
    private Enemy_Gunner gunnerParent;
    // Start is called before the first frame update
    void Awake()
    {
        gunnerParent = GetComponentInParent<Enemy_Gunner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

        gameObject.SetActive(false);
        gunnerParent.target = collision.transform;
        gunnerParent.inRange = true;
        gunnerParent.hotZone.SetActive(true);
        }
    }
}
