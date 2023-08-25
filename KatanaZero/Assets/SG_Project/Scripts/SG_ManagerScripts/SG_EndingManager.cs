using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class SG_EndingManager : MonoBehaviour
{
    public GameObject endingText001;
    public GameObject endingText002;

    public GameObject[] lightsObj;
    public GameObject globalLightObj;
    public GameObject userInterface;
    public GameObject player;


    SG_Doctor doctorClass;

    Light2D globalLight;

    Coroutine Coroutine;

    Color startColor;
    Color endColor;

    float startTime = 0f;
    float endTime = 3f;

    private bool isEnding = false;



    // Start is called before the first frame update
    void Start()
    {
        
       globalLight = globalLightObj.GetComponent<Light2D>();

        doctorClass = FindAnyObjectByType<SG_Doctor>();

        startColor = new Color(1f, 1f, 1f, 1f);
        endColor = new Color(0f, 0f, 0f, 1f);       

        

        //Coroutine = StartCoroutine(StartEnding());
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnding == false && doctorClass.isCutinEnding == true)
        {
            isEnding = true;
            Coroutine = StartCoroutine(StartEnding());
        }

    }


    IEnumerator StartEnding()
    {       

        // 나머지 빛 다 꺼주기
        for(int i = 0; i < lightsObj.Length; i++)
        {
            lightsObj[i].SetActive(false);
        }
        
        userInterface.SetActive(false);

        // 화면 점점 어두워지게
        startTime = 0f;
        while (startTime < endTime)
        {
            startTime += Time.deltaTime;

            float time = Mathf.Clamp01(startTime / endTime);

            globalLight.color = Color.Lerp(startColor, endColor, time);

            yield return null;
        }

        // 이제 텍스트 출력 해야함
        yield return new WaitForSeconds(0.5f);

        endingText001.SetActive(true);       

        yield return new WaitForSeconds(4f);
        endingText001.SetActive(false);

        endingText002.SetActive(true);

        yield return new WaitForSeconds(4f);
        endingText002.SetActive(false);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("TitleScene");



    }

}
