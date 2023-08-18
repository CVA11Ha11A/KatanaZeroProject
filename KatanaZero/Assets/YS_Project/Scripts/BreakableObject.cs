using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    GameObject breakable;
    // Start is called before the first frame update
    void Start()
    {
        breakable = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BreakPlatform()
    {
        gameObject.SetActive(false);
    }
}
