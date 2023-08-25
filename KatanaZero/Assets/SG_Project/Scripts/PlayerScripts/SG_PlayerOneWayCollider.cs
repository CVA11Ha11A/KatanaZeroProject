using Rewired;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SG_PlayerOneWayCollider : MonoBehaviour
{
    Player player;

    private GameObject oneWayPlatFormObj;

    [SerializeField] private BoxCollider2D playerCollider;


    private bool leftKey = false;
    private bool rightKey = false;

    private Coroutine boxingDisableCollision;

    private WaitForSeconds waitForSeconds;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);

        waitForSeconds = new WaitForSeconds(0.50f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogFormat("Left -> {0} Right -> {1}", leftKey, rightKey);
        InputMethod();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            oneWayPlatFormObj = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        oneWayPlatFormObj = null;
    }

    private IEnumerator DisableCollision()
    {

        BoxCollider2D platFormCollider = oneWayPlatFormObj.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platFormCollider, true);

        yield return waitForSeconds;

        Physics2D.IgnoreCollision(playerCollider, platFormCollider, false);
    }

    private void InputMethod()
    {
        if (player.GetButtonDown("Down") && leftKey == false && rightKey == false)
        {
            if (oneWayPlatFormObj != null)
            {
                boxingDisableCollision = StartCoroutine(DisableCollision());
            }
        }


        if (player.GetButtonDown("MoveRight"))
        {
            rightKey = true;
        }
        else { /* PASS */}

        if (player.GetButtonUp("MoveRight"))
        {
            rightKey = false;
        }
        else { /* PASS */}

        if (player.GetButtonDown("MoveLeft"))
        {
            leftKey = true;
        }
        else { /* PASS */}

        if (player.GetButtonUp("MoveLeft"))
        {
            leftKey = false;
        }
        else { /* PASS */}
    }

}
