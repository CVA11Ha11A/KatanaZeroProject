using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kissyface_Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            Debug.Log("Player≈∏∞›");
            PlayerMove playerMove = collision.GetComponent<PlayerMove>();
            if(playerMove!=null)
            {
                if(playerMove.isDodge==false&&playerMove.isDie==false)
                {
                playerMove.Die();

                }
            }
        }
    }
}
