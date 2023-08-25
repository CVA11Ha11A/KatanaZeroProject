using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSoundEffect : MonoBehaviour
{
    AudioClip playClip;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReflectSound(AudioClip clip_)
    {
        source.clip = clip_;
        source.Play();
    }
}
