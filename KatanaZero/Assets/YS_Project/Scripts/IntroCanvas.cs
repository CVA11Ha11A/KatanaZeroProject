using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class IntroCanvas : MonoBehaviour
{
    public static IntroCanvas instance;
    Player player;
    public bool isIntroOver = false;
    public GameObject musicUi;
    public GameObject stageUi;
    public GameObject timeManager;
    IntroManager introManager;
    WaitForSeconds introTime = new WaitForSeconds(3);

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        introManager = FindAnyObjectByType<IntroManager>();
        if (introManager.introOver)
        {
            gameObject.SetActive(true);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }
    private void OnEnable()
    {
        
        StartCoroutine(IntroUiSet());
    }

    // Update is called once per frame
    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "BossScene" || currentSceneName == "TitleScene")
        {
            Destroy(gameObject);
        }
        if (player.GetButtonDown("Attack"))
        {
            this.gameObject.SetActive(false);
            isIntroOver = true;
            timeManager.SetActive(true);
        }
    }
    private IEnumerator IntroUiSet()
    {
        musicUi.SetActive(true);
        yield return introTime;
        stageUi.SetActive(true);
    }
}
