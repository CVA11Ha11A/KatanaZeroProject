using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflect : MonoBehaviour
{
    public GameObject reflectEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        reflectEffect.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Bullet"))
        {
            reflectEffect.SetActive(true);


        }
    }
}
