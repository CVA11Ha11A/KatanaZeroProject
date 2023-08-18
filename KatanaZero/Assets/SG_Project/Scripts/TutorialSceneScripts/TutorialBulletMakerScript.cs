using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBulletMakerScript : MonoBehaviour
{
    public GameObject bulletPrefab;

    private GameObject bulletClone;

    private Animator animator; 

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
        animator = GetComponent<Animator>();
        animator.Play("EnemyGunEmpect");
        bulletClone  = Instantiate(bulletPrefab,this.transform.position,this.transform.localRotation);
        this.gameObject.SetActive(false);

    }

    public void OnDisable()
    {
        
    }
}
