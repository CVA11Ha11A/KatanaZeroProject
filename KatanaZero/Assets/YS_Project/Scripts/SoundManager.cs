using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    
   
    [SerializeField] private AudioClip[] slashClip;
    [SerializeField] private AudioClip rewindClip;
    private static SoundManager instance;
    AudioSource soundEffect;
    private bool rewindOn=false;
    TimeBody timeBody;

   
    // Start is called before the first frame update
    void Start()
    {
        timeBody = FindAnyObjectByType<TimeBody>();
         soundEffect = GetComponent<AudioSource>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    public void AttackSound()
    {
        int randomIdx = Random.Range(0, 4);
        soundEffect.clip = slashClip[randomIdx];
        soundEffect.Play();
    }
    public void RewindSound()
    {
        
        if(timeBody.isRewindOver==false)
        {
            rewindOn = true;
            soundEffect.clip = rewindClip;
            if(soundEffect.isPlaying==false)
            {
            soundEffect.Play();

            }


        }
    }
}
