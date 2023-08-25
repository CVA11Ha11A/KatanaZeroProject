using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SG_WarningLightScprit : MonoBehaviour
{    
    Light2D light2D;

    Color32 minLightColor32;
    Color minLightColor;

    Color32 maxLightColor32;
    Color maxLightColor;

    Coroutine coroutine;

    float timeElapsed;
    float duration;

    // Start is called before the first frame update
    void Start()
    {
        light2D = this.GetComponent<Light2D>();
        //maxLight = new Color(255, 33, 0, 100);
      
        // Color32 선언
        minLightColor32 = new Color32(255, 33, 0, 30);
        // Color32 -> Color로 구하기
        minLightColor = new Color(
            minLightColor32.r / 255f,
            minLightColor32.g / 255f,
            minLightColor32.b / 255f,
            minLightColor32.a / 255f);

        // Color32 선언
        maxLightColor32 = new Color32(255, 33, 0, 100);
        // Color32 -> Color로 구하기
        maxLightColor = new Color(
            maxLightColor32.r / 255f,
            maxLightColor32.g / 255f,
            maxLightColor32.b / 255f,
            maxLightColor32.a / 255f);

        light2D.color = minLightColor;

        timeElapsed = 0f;
        duration = 1.5f;


        coroutine = StartCoroutine(LightUp());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LightUp()
    {
        timeElapsed = 0f;        
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / duration);

            light2D.color = Color.Lerp(minLightColor, maxLightColor, time);

            yield return null;
        }

        coroutine = StartCoroutine(LightDown());

       
    }
    private IEnumerator LightDown()
    {
        timeElapsed = 0f;
        
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / duration);

            light2D.color = Color.Lerp(maxLightColor,minLightColor , time);
            
            yield return null;
        }

        coroutine = StartCoroutine(LightUp());

    }
}
