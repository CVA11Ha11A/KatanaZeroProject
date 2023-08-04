using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool isGhostMake;

    // Start is called before the first frame update
    void Start()
    {
        isGhostMake = true;
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGhostMake)
        {

            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;

            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                currentGhost.transform.localScale = this.transform.localScale;
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 0.4f);
            }
        }
    }
}
