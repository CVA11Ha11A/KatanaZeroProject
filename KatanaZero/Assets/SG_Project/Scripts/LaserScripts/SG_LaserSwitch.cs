using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_LaserSwitch : MonoBehaviour
{
    Switch switchClass;

    public GameObject laserLine;
    

    private bool isSwitchBottonOn = true;
    

    

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

    public void IsChangedBool(bool buttonSwitch)
    {
        isSwitchBottonOn = buttonSwitch;

        //Debug.Log("buttonSwitch 변화해서 불러왔나?");
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
