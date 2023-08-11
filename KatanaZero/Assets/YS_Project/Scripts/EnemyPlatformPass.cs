using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPlatformPass : MonoBehaviour
{
    public bool isPass;
    private GameObject currentPlatform;
    [SerializeField] private BoxCollider2D enemyCollider;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (isPass)
        {

            if (currentPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("enemy∞° platformø° ¥Í¿Ω");
            currentPlatform = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(enemyCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(enemyCollider, platformCollider, false);
    }
}
