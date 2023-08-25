using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_MoveRedLaser : MonoBehaviour
{
    //--------------------------------------- !움직일 필요 없는 레이져라면 이거 꺼야됨! ----------------------------------
    Switch switchClass;

    private float moveSpeed = 3f;

    public bool leftCrash = false;
    public bool rightCrash = false;
    public bool laserIsSwitchBottonOn = true;

    private float daltatime = 0;

    private Vector3 leftMove = default;
    private Vector3 rightMove = default;



    // Start is called before the first frame update
    void Start()
    {
        MoveRedLaserInitialization();
        switchClass = FindObjectOfType<Switch>();
        switchClass.switchButtionboolChanged += IsChangedBool;
    }

    // Update is called once per frame
    void Update()
    {
        daltatime += moveSpeed * Time.deltaTime;
        LaserMovement();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 레이저 상단 끼리 부딪혔을떄
        if (collision.gameObject.CompareTag("SG_Laser"))
        {
            // { 왼쪽가는중인데 부딪힌거면 오른쪽으로 가게 하고 오른쪽으로 가다가 부딪힌거면 왼쪽으로 가게함
            if (leftCrash == true)
            {
                leftCrash = false;
                rightCrash = true;
            }
            else if (rightCrash == true)
            {
                leftCrash = true;
                rightCrash = false;
            }
            else { /*PASS*/ }
            // } 왼쪽가는중인데 부딪힌거면 오른쪽으로 가게 하고 오른쪽으로 가다가 부딪힌거면 왼쪽으로 가게함
        }
        else { /*PASS*/ }
    }

    public void LaserMovement()
    {
        if (laserIsSwitchBottonOn == true) // 스위치가 On일때만 움직이게
        {

            // 오른쪽에 부딪히면 왼쪽으로 이동
            if (leftCrash == false && rightCrash == true)
            {
                //조건에 맞으면 움직임
                if (daltatime >= 0.1)
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + leftMove;
                    daltatime = 0;
                }


            }

            // 왼쪽에 부딪히면 오른쪽으로 이동
            else if (leftCrash == true && rightCrash == false)
            {
                // 조건에 맞으면 움직임
                if (daltatime >= 0.1)
                {
                    this.gameObject.transform.position = this.gameObject.transform.position + rightMove;
                    daltatime = 0;
                }


            }
            else
            {
                Debug.Log("무언가 잘못되었다");
            }

        }
    }

    public void IsChangedBool(bool buttonSwitch)
    {
        laserIsSwitchBottonOn = buttonSwitch;

        //Debug.Log("buttonSwitch 변화해서 불러왔나?");
    }

    // 첫 선언 모음
    public void MoveRedLaserInitialization()
    {

        int firstMove = Random.Range(0, 1);

        //  첫움직임은 랜덤으로
        if (firstMove == 0)
        {
            leftCrash = true;
        }
        else if (firstMove == 1)
        {
            rightCrash = true;
        }
        else { /*PASS*/ }

        // 왼쪽으로 이동할떄 빼줄 값
        if (leftMove == default || leftMove == null)
        {
            leftMove = new Vector3(-0.05f, 0f, 0f);
        }
        else { /*PASS*/ }

        // 오른쪽으로 이동할때 빼줄 값
        if (rightMove == default || rightMove == null)
        {
            rightMove = new Vector3(0.05f, 0f, 0f);
        }
        else { /*PASS*/ }


    }
}   //  NameSpace    
