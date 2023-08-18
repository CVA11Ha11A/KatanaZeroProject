using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_FanObj : MonoBehaviour
{
    SpriteRenderer fanSprite;
    BoxCollider2D fanCollider;


    // Start is called before the first frame update
    void Start()
    {
        fanSprite = GetComponent<SpriteRenderer>();
        fanCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ColliderControler();
        //Debug.LogFormat(fanSprite.sprite.name);
        //spr_fanblade_0 ~ 31
    }

    public void ColliderControler()
    {
        if (fanSprite.sprite.name == "spr_fanblade_10")
        {
            fanCollider.enabled = false;
        }
        else { /*PASS*/ }

        if (fanSprite.sprite.name == "spr_fanblade_21")
        {
            fanCollider.enabled = true;
        }
    }

}
