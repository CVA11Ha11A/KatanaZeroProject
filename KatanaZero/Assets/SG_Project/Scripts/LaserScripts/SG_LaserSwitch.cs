using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_LaserSwitch : MonoBehaviour
{
    Switch switchClass;
    SG_RedDottedLineControler redDottedLineClass;
    public GameObject laserLine;

    //public event Action<bool> IsAttackingEvent;
    

    private bool isSwitchBottonOn = true;


    private bool isAttacking = false;

    //public bool IsAttackint
    //{
    //    get { return isAttacking; }

    //    set
    //    {
    //        if(isAttacking != value)
    //        {
    //            isAttacking = value;
    //            IsAttackingEvent?.Invoke(isAttacking);
    //        }
    //    }
    //}

    

    public void Awake()
    {
        
    }

    void Start()
    {
        switchClass = FindObjectOfType<Switch>();
     

        switchClass.switchButtionboolChanged += IsChangedBool;
        //Debug.LogFormat("Class가 잘 들어갔는지 -> {0}", switchClass);
    }

    
    void Update()
    {
        ChildObjControl();       
    }

    public void LateUpdate()
    {

    }
    public void IsChangedBool(bool buttonSwitch)
    {
        isSwitchBottonOn = buttonSwitch;

        //Debug.Log("buttonSwitch 변화해서 불러왔나?");
    }

    public void RedottedLineGetBool(bool redDottedbool)
    {
        isAttacking = redDottedbool;
    }

    void ChildObjControl()
    {       
        if(isSwitchBottonOn == true)
        {
           laserLine.gameObject.SetActive(true);
        }
        else if (isSwitchBottonOn == false)
        {
            laserLine.gameObject.SetActive(false);
        }
    }
}
