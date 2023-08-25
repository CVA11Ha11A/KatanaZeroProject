using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ClearText001 : MonoBehaviour
{
    Color firstAlpha;

    TextMeshProUGUI text;
    public GameObject playerUI;
    public GameObject[] lights;

    Color startColor;
    Color endColor;

    float timeElapsed;
    float duration;

    Coroutine coroutine;

    public event Action<bool> endClearTextEvent;

    private bool endClearText = false;

    public bool EndClearText
    {
        get { return endClearText; }

        set
        {
            if (endClearText != value)
            {
                endClearText = value;
                endClearTextEvent?.Invoke(endClearText);
            }
            else { /*PASS*/ }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();

        startColor = new Color(1, 1, 1, 0);
        endColor = new Color(1, 1, 1, 1);
        text.color = startColor;
        timeElapsed = 0.0f;
        duration = 2f;

        coroutine = StartCoroutine(MoreAndMoreWhite());
    }

    public void OnDisable()
    {
        
    }

    private IEnumerator MoreAndMoreWhite()
    {
       playerUI.SetActive(false);
        for(int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / duration);

            text.color = Color.Lerp(startColor, endColor, time);

            yield return null;
        }

        coroutine = StartCoroutine(MoreAndMoreWhiteAfter());

    }

    private IEnumerator MoreAndMoreWhiteAfter()
    {
        timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float time = Mathf.Clamp01(timeElapsed / duration);

            text.color = Color.Lerp(endColor, startColor, time);

            yield return null;
        }

        EndClearText = true;

        this.gameObject.SetActive(false);
    }

}
