using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class GameOverUI : MonoBehaviour
{
    Player player;
    TimeBody timeBody;
   
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        timeBody = FindAnyObjectByType<TimeBody>();

    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetButtonDown("Attack"))
        {
            Rewind();
            
        }
    }
    void Rewind()
    {
        TimeBody[] timeBodies = FindObjectsOfType<TimeBody>();
        foreach (TimeBody timeBody in timeBodies)
        {
            timeBody.isRewindin = true;
        }
        gameObject.SetActive(false);
    }
}
