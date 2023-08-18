using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class IntroCanvas : MonoBehaviour
{
    Player player;
    public bool isIntroOver = false;
    public GameObject musicUi;
    public GameObject stageUi;
    public GameObject timeManager;
    WaitForSeconds introTime = new WaitForSeconds(3);

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
        if(player.GetButtonDown("Attack"))
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
