using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SG_AttackImfact : MonoBehaviour
{

    public Transform player;
    private Animator empectAni;
    private SG_PlayerMovement playerMovementClass;
    private BoxCollider2D boxCollider;

    private float pitagorasC;
    private float playerZ;
    private float mouseZ;

    private bool endTutorial = false;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        //this.gameObject.SetActive(false);
        empectAni = GetComponent<Animator>();
        playerMovementClass = FindObjectOfType<SG_PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // PlayerIsClick();
        TutorialOneAttack();
        PlayerIsClick001();
    }
    public void LateUpdate()
    {
       
    }

    private void PlayerIsClick()
    {
        #region 공격시 Active조절을 위한 임시주석처리
        if (Input.GetMouseButtonDown(0))
        {
            if (playerMovementClass.leftClickAttackCoolTime == false && playerMovementClass.attackCount < 4)
            {
                #region LEGACY
                /*      LEGACY
                //playerZ = this.transform.position.x + this.transform.position.y;
                //mouseZ = playerMovementClass.mousePosition.x + playerMovementClass.mousePosition.y;

                //pitagorasC = playerZ + mouseZ;

                //// 피타고라스씨 디버그 찍기
                //Debug.LogFormat("피타고라스씨 -> {0}", pitagorasC);
                ////this.gameObject.transform.LookAt(playerMovementClass.mousePosition);
                ////this.gameObject.transform.localRotation = Quaternion.Euler(this.gameObject.transform.localRotation.x,0f, this.gameObject.transform.localRotation.z);

                //this.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, pitagorasC);
                ////this.gameObject.transform.Rotate(new Vector3(0f, 0f, playerMovementClass.mousePosition.z));
                         LEGACY */
                #endregion

                var dir = playerMovementClass.mousePosition - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                //Debug.LogFormat("mouse -> {0}", playerMovementClass.mousePosition);
                //Debug.LogFormat("player -> {0}", transform.position);
                //Debug.LogFormat("dir -> {0}", dir);

                transform.localScale = player.localScale;
                

                empectAni.SetTrigger("Slash");
            }
        }
        #endregion
    }
    private void PlayerIsClick001()
    {
       
            if (playerMovementClass.one == 1)
            {
                playerMovementClass.one = 0;
                var dir = playerMovementClass.mousePosition - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
       
                transform.localScale = player.localScale;


                empectAni.SetTrigger("Slash");
            }
        

    }

    private void TutorialOneAttack()
    {
        if(endTutorial == false)
        {
            boxCollider.enabled = false;
            //if(Time.timeScale <= 0.2)
            //{
            //    boxCollider.enabled = true;
            //}

            if (Time.timeScale == 0f && Input.GetMouseButtonDown(0))
            {                  
                boxCollider.enabled = true;
                transform.localScale = new Vector3(-1f,1f,1f);
                empectAni.Play("SlashAnimaition");
                endTutorial = true;
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

        }
        
    }

}       // NameSpace
