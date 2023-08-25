using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.Universal;
using Rewired;

public class LaserPassage002Map : MonoBehaviour
{
    // 클리어시 꺼줄 오브젝트
    public GameObject[] lights;
    public GameObject playerUI;

    // { 글로벌 라이트 관련 선언
    public GameObject globalLightObj;
    private Light2D globalLight;

    // Start Color = 1 -> 0
    private Color globalLightStartColor;
    private Color globalLightEndColor;

    // 러프를 위한 변수들
    private float lightStartTime = 0;
    private float lightEndTime = 2;

    // } 글로벌 라이트 관련 선언

    // { 텍스트 관련 선언
    public TextMeshProUGUI clearText;

    // A값 0 -> 1
    private Color clearTextStartColor;
    private Color clearTextEndColor;

    private float textStartTime = 0;
    private float textEndTime = 2;
    // } 텍스트 관련 선언

    Coroutine lightCoroutine;
    Coroutine textCoroutine;




    // 다음씬이동을 한번만 시켜주기 위한 변수
    private bool laserPassageClear = false;

    private int sceneIdx;



    void Start()
    {
        globalLight = globalLightObj.GetComponent<Light2D>();
        //clearText = GetComponent<TextMeshProUGUI>();

        // 처음에 텍스트 투명하게 해서 클리어 할때에만 보이게 만듦
        clearText.color = clearTextStartColor;

        globalLightStartColor = new Color(1, 1, 1, 1);
        globalLightEndColor = new Color(0, 0, 0, 1);

        clearTextStartColor = new Color(1, 1, 1, 0);
        clearTextEndColor = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && laserPassageClear == false)
        {
            laserPassageClear = true;

            lightCoroutine = StartCoroutine(GlobalLightOff());
            textCoroutine = StartCoroutine(ClearTextOn());

        }
    }

    // 글로벌라이트를 이용해서 화면 점점 검은색으로 변하는 코루틴
    IEnumerator GlobalLightOff()
    {
        // 화면 검은색되는데 방해되는 오브젝트들 끄기
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        playerUI.SetActive(false);

        while (lightStartTime < lightEndTime)
        {
            lightStartTime += Time.deltaTime;

            float time = Mathf.Clamp01(lightStartTime / lightEndTime);

            globalLight.color = Color.Lerp(globalLightStartColor, globalLightEndColor, time);

            yield return null;
        }

    }

    IEnumerator ClearTextOn()
    {
        while (textStartTime < textEndTime)
        {
            textStartTime += Time.deltaTime;

            float textTime = Mathf.Clamp01(textStartTime / textEndTime);

            clearText.color = Color.Lerp(clearTextStartColor, clearTextEndColor, textTime);

            yield return null;
        }

        yield return new WaitForSeconds(3);

        // 현재씬 Index를 얻고 거기에 1을 더한 씬을 부르게 하는 로직
        if (EnemyCountManager.Instance.isAllClear)
        {
            sceneIdx = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIdx + 1);
        }


    }


    //if (collision.tag.Equals("Player"))
    //{        
    //    if (EnemyCountManager.Instance.isAllClear)
    //    {
    //        sceneIdx = SceneManager.GetActiveScene().buildIndex;
    //        SceneManager.LoadScene(sceneIdx + 1);
    //    }
    //}
}
