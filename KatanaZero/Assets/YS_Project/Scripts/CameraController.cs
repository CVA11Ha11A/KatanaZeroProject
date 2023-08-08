using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   public Transform target;
    Vector3 cameraPosition = new Vector3(0f, 5f, -30f);
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {

        //if(target.position.y>1.9f)
        //{
        //    transform.position =new Vector3(0f, 3.5f,-10f);
        //}
        //else
        //{
        //    transform.position = new Vector3(0f,0, -10f);

        //}
        Vector3 camera = new Vector3(target.position.x, 2, -10f);
        transform.position = camera;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
