using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public AudioClip selectClip;
    AudioSource audioSource;
    SceneManager manager;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        audioSource.clip = selectClip;
        audioSource.Play();
        SceneManager.LoadScene("Tutorial");
    }
    public void BossRoom()
    {
        audioSource.clip = selectClip;
        audioSource.Play();
        SceneManager.LoadScene("BossScene");
    }
    public void QuitGame()
    {
        audioSource.clip = selectClip;
        audioSource.Play();
        Application.Quit();
    }

}
