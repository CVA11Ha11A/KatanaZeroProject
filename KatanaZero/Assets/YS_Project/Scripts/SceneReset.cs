using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneReset : MonoBehaviour
{
    public TimeBody timeBody;
   

    // Start is called before the first frame update
    void Start()
    {
        timeBody = FindAnyObjectByType<TimeBody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBody.isRewindOver)
        {
            //EnemyRay[] enemyRays = FindObjectsOfType<EnemyRay>();

            //foreach (EnemyRay enemy in enemyRays)
            //{
            //    enemy.Init();
            //}
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
