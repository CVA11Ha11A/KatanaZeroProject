using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  AudioClip cassetClip;
    public  AudioClip backgroundClip;
    public  AudioSource bgm;
    // Start is called before the first frame update
    void Start()
    {
        bgm.clip = backgroundClip;
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
