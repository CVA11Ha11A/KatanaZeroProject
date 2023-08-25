using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float spinSpeed = 1000f;
    public bool isStop = false;
    public bool isWall = false;
    float soundTimer = 0;
    float soundRate = 0.2f;
    AudioSource soundSource;
    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       if(isStop)
        {
            soundTimer = 0;
        }
       else
        {
            soundTimer += Time.deltaTime;

        }
    
        if(soundTimer>=soundRate)
        {
            soundSource.Play();
            soundTimer = 0;
        }
        if(!isStop&&!isWall)
        {
        float rotationAmount = spinSpeed * Time.deltaTime;
        transform.Rotate(Vector3.back, rotationAmount);

        }
        if(!isStop&&isWall)
        {
            float rotationAmount = spinSpeed * Time.deltaTime;
            transform.Rotate(Vector3.back, -rotationAmount);
        }
    }
    public void StopRotate()
    {
        isStop = true;
    }
}
