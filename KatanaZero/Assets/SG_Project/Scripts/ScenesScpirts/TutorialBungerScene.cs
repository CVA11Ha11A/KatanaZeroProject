using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialBungerScene : MonoBehaviour
{
    public TextMeshProUGUI clearText;
    public Image backGroundImage;
    Coroutine coroutine;

    int sceneIdx;

    private bool isNextScene = false;


    private float startImgTime = 0f;
    private float endImgTime = 2f;

    private float startTextTime = 0f;
    private float endTextTime = 2f;


    // Text는 A값 조정 [흰] 0 -> 1
    Color clearTextStartColor;
    Color clearTextEndColor;

    // Img 는 A값 조정 [검] 0 -> 1
    Color clearImgStartColor;
    Color clearImgEndColor;

    // Start is called before the first frame update
    void Start()
    {
        clearTextStartColor = new Color(1, 1, 1, 0);
        clearTextEndColor = new Color(1, 1, 1, 1);
        clearImgStartColor = new Color(0, 0, 0, 0);
        clearImgEndColor = new Color(0, 0, 0, 1);

        clearText.color = clearTextStartColor;
        backGroundImage.color = clearImgStartColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && isNextScene == false)
        {
            isNextScene = true;
            coroutine = StartCoroutine(NextSceneBackGround());
            coroutine = StartCoroutine(NextSceneText());
        }
    }

    IEnumerator NextSceneBackGround()
    {
        startImgTime = 0f;
        while (startImgTime < endImgTime)
        {
            startImgTime += Time.deltaTime;

            float time = Mathf.Clamp01(startImgTime / endImgTime);

            backGroundImage.color = Color.Lerp(clearImgStartColor, clearImgEndColor, time);

            yield return null;
        }        
    }

    IEnumerator NextSceneText()
    {
        startTextTime = 0f;
        while(startTextTime < endTextTime)
        {
            startTextTime += Time.deltaTime;

            float textTime = Mathf.Clamp01(startTextTime / endTextTime);

            clearText.color = Color.Lerp(clearTextStartColor, clearTextEndColor, textTime);

            yield return null;
        }

        yield return new WaitForSeconds(2);

        if (EnemyCountManager.Instance.isAllClear)
        {
            sceneIdx = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIdx + 1);
        }

    }

    //if (EnemyCountManager.Instance.isAllClear)
    //{
    //    sceneIdx = SceneManager.GetActiveScene().buildIndex;
    //    SceneManager.LoadScene(sceneIdx + 1);
    //}
}
