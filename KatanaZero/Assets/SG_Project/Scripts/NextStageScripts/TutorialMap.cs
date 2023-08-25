using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class TutorialMap : MonoBehaviour
{
    public GameObject tutorialBackLightObj;

    public TextMeshProUGUI clearText;

    private Light2D tutorialBackLight;

    private Coroutine backGroundColorChange;



    private Scene nowScene;

    private Color MinusbackGround;

    private Color clearTextStartColor;
    private Color clearTextEndColor;

    private bool isGoalInPlayer = false;

    int sceneIdx;

    // 0.1 초로 초기화 했음
    private WaitForSeconds waitForSeconds;

    // Start is called before the first frame update
    void Start()
    {
        nowScene = SceneManager.GetActiveScene();
        waitForSeconds = new WaitForSeconds(0.2f);
        tutorialBackLight = tutorialBackLightObj.GetComponent<Light2D>();
        FirstInItColor();

        clearTextStartColor = new Color(1, 1, 1, 0);
        clearTextEndColor = new Color(1, 1, 1, 1);
        clearText.color = clearTextStartColor;

    }

    // Update is called once per frame
    void Update()
    {
        IsTutorialSceneGole();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isGoalInPlayer = true;
        }
        else { /*PASS*/ }

    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isGoalInPlayer = true;
        }
        else { /*PASS*/ }

    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isGoalInPlayer = false;
        }
        else { /*PASS*/ }

    }

    private void IsTutorialSceneGole()
    {
        if (nowScene.name == ("Tutorial"))
        {
            if (isGoalInPlayer == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    // 화면 검은색으로 변하면서 다 검은색이 된다면 다음 씬 로드

                    backGroundColorChange = StartCoroutine(BackGroundColorChange());

                }
            }
        }
    }

    private void FirstInItColor()
    {
        // 처음에 컬러값 넣어주기
        MinusbackGround.r = tutorialBackLight.color.r;
        MinusbackGround.g = tutorialBackLight.color.g;
        MinusbackGround.b = tutorialBackLight.color.b;
    }


    private IEnumerator BackGroundColorChange()
    {
        //int minusNum = 2;

        //for (int i = 0; i <= 126; i++)
        //{
        //    MinusbackGround.r = MinusbackGround.r - minusNum;
        //    MinusbackGround.g = MinusbackGround.g - minusNum;
        //    MinusbackGround.b = MinusbackGround.b - minusNum;

        //    tutorialBackLight.color = MinusbackGround;

        //    yield return waitForSeconds;

        //}
        //MinusbackGround.r = 0;
        //MinusbackGround.g = 0;
        //MinusbackGround.b = 0;

        //tutorialBackLight.color = MinusbackGround;
        //for (int j = 0; j <= 3; j++)
        //{
        //    yield return waitForSeconds;
        //}

        float timeElapsed = 0.0f;
        float duration = 2f;

        Color colorEnd = new Color(1, 1, 1, 1);
        Color colorStart = new Color(0, 0, 0, 1);

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;            

            float time = Mathf.Clamp01(timeElapsed / duration);

            tutorialBackLight.color = Color.Lerp(colorEnd, colorStart, time);
            yield return null;
        }

        timeElapsed = 0.0f;

        //글자 켜지기
        while (timeElapsed < duration)
        {
            Debug.Log("글씨 나오는 로직 들어가나?");
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / duration);

            clearText.color = Color.Lerp(clearTextStartColor, clearTextEndColor, time);
            yield return null;
        }

        //다음 씬으로
        if (EnemyCountManager.Instance.isAllClear)
        {            
            sceneIdx = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIdx + 1);
        }

        

    }

}   //NAMESPACE

