using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public  AudioClip cassetClip;
    public  AudioClip backgroundClip;
    public AudioClip SlowmotionClip;
    public  AudioSource bgm;
    public TMP_Text songName;
    public GameObject introUi;
    private bool isSlow=false;
    public IntroCanvas introCanvas;

    TimeManager timeManager;
    // Start is called before the first frame update
    void Start()
    {
        bgm.clip = backgroundClip;
        
    }

    // Update is called once per frame
    void Update()
    {
      

       if(introCanvas.isIntroOver==false)
        {
            return;
        }
        
        
        timeManager = FindAnyObjectByType<TimeManager>();
       if( timeManager.isTimeSlow==true)
        {
            if(isSlow==false)
            {
           
                isSlow = true;
            }
            bgm.pitch = 0.4f;
        }
       else
        {
            bgm.pitch = 1f;
            isSlow = false;
        }
    }
    public void IntroAction()
    {
        bgm.Stop();
        bgm.PlayOneShot(cassetClip);
        StartCoroutine(IntroTimer());
    }
    private IEnumerator IntroTimer()
    {
        yield return new WaitForSeconds(3);
        if (!bgm.isPlaying)
        {
            bgm.Play();
        }
        songName.text = string.Format("{0}", backgroundClip.name);
        introUi.SetActive(true);

    }
}
