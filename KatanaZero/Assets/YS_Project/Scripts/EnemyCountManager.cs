using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountManager : MonoBehaviour
{
    private static EnemyCountManager _instance;

    public static EnemyCountManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyCountManager>();
            }
            return _instance;
        }
    }
    public int maxCount;
    public int currentCount;
    public bool isAllClear=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentCount==maxCount)
        {
            isAllClear = true;
        }
    }

   
}
