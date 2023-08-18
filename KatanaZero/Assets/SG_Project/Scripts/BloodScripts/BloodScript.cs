using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScript : MonoBehaviour
{
    public Sprite[] bloodSprite = new Sprite[15];

    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rigid;
    private BoxCollider2D boxCollider;

    int randNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        randNum = Random.Range(0, 14);

        spriteRenderer.sprite = bloodSprite[randNum];

        Destroy(this.gameObject, 5f);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            Destroy(rigid);
            //boxCollider.enabled = false;
        }
    }
}
