using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoalLightScript : MonoBehaviour
{
    Light2D thisLight;
    public GameObject clearText;
    public GameObject lights;
    Coroutine moreAndMoreBlack;

    WaitForFixedUpdate waitForFixed;

    private bool isCoroutineing;

    // Start is called before the first frame update
    void Start()
    {
        thisLight = this.gameObject.GetComponent<Light2D>();
        waitForFixed = new WaitForFixedUpdate();
        isCoroutineing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isCoroutineing == false)
            {
                moreAndMoreBlack = StartCoroutine(MoreAndMoreBlack());
            }

        }
    }

    private IEnumerator MoreAndMoreBlack()
    {
        isCoroutineing = true;
        lights.SetActive(false);
        //텍스트 켜주기
        clearText.SetActive(true);

        float timeElapsed = 0.0f;
        float duration = 1.8f;

        Color colorEnd = new Color(1, 1, 1, 1);
        Color colorStart = new Color(0, 0, 0, 1);

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / duration);

            thisLight.color = Color.Lerp(colorEnd, colorStart, time);

            yield return null;
        }

        // 5 = 0.1
        for (int i = 0; i <= 100; i++)
        {
            yield return waitForFixed;
        }



        // 갑자기 밝아져서 임시 주석
        //timeElapsed = 0;

        //while (timeElapsed < duration)
        //{
        //    timeElapsed += Time.deltaTime;

        //    float time = Mathf.Clamp01(timeElapsed / duration);

        //    thisLight.color = Color.Lerp(colorStart, colorEnd, time);

        //    yield return null;
        //}


    }

    //LEGACY
    //private void SlowBackScreen()
    //{
    //    float timeElapsed = 0.0f;
    //    float duration = 2f;

    //    Color colorEnd = new Color(1, 1, 1, 1);
    //    Color colorStart = new Color(0, 0, 0, 1);

    //    while (timeElapsed < duration)
    //    {
    //        timeElapsed += Time.deltaTime;

    //        float time = Mathf.Clamp01(timeElapsed / duration);

    //        thisLight.color = Color.Lerp(colorEnd, colorStart, time);
    //    }
    //}
    //LEGACY
}
