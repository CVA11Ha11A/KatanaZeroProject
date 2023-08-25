using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    int sceneIdx;
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
        int sceneIdx;
        if (collision.tag.Equals("Player"))
        {        
            if (EnemyCountManager.Instance.isAllClear)
            {
                sceneIdx = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(sceneIdx + 1);
            }
        }
    }
}
