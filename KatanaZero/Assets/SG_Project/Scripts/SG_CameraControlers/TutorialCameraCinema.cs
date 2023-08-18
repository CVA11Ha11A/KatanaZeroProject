using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TutorialCameraCinema : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    private CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera firstVirtual;
    public GameObject player;


    float fllowSpeed = 3.0f;



    void Start()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        mainCamera = Camera.main;


    }

    
    void Update()
    {

        //if(Input.anyKey)
        //{

        //    firstVirtual.Follow = player.transform;
        //}



    }

    //private void FollowPlayer()
    //{
    //    while(firstVirtual.transform != player.transform)
    //    {
    //        firstVirtual.transform.position = fllowSpeed * Time.deltaTime;
    //    }
    //}
}
