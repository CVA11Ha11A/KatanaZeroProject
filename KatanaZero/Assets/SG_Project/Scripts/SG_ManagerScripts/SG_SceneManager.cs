using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SG_SceneManager : MonoBehaviour
{

    //Scene scene;
    Scene nowScene;

    ClearText001 clearText001Script;

    // LaserPassage 변수
    private bool isLaserPassageSceneClear = false;
    private bool laserPassageGet = false;
    public GameObject laserPassageSceneTextObj;

    // Start is called before the first frame update
    public void Awake()
    {
        // 현재씬 삽입
        nowScene = SceneManager.GetActiveScene();




    }

    void Start()
    {        

    }

    // Update is called once per frame
    void Update()
    {
        GetInstance();
        ChangeScene();
    }

    // ----------------------- 클리어시 이동시킬 씬 ---------------------------

    public void ChangeScene()
    {
        if (nowScene.name == ("LaserPassage") && isLaserPassageSceneClear == true)
        {
            SceneManager.LoadScene("SG_LaserPassage002");
        }
        else { /*PASS*/ }
    }

    // ---------------------- 텍스트 오브젝트가 켜지면 인스턴스 -------------------------

    public void GetInstance()
    {
        #region LaserPassage
        if (nowScene.name == ("LaserPassage")) // LaserPassage
        {
            if (laserPassageSceneTextObj.activeSelf == true && laserPassageGet == false)
            {
                laserPassageGet = true;
                // LaserPassageScene 씬 이벤트 가져오기위한 Get
                clearText001Script = FindObjectOfType<ClearText001>();
                // LaserPassageScene 클리어 이벤트 구독                
                clearText001Script.endClearTextEvent += IsLaserPassageSceneClear;
            }
        }
        #endregion LaserPassage



    }


    // ---------------------- 이벤트 구독 함수 --------------------------------
    public void IsLaserPassageSceneClear(bool eventValue)
    {
        isLaserPassageSceneClear = eventValue;
        //Debug.LogFormat("이벤트가 잘 불러와져서 바뀌었나? -> {0}", isLaserPassageSceneClear);
    }


}
