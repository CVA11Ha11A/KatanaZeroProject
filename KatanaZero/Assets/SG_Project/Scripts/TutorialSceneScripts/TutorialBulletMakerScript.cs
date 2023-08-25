using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TutorialBulletMakerScript : MonoBehaviour
{
    public GameObject bulletPrefab;

    private GameObject bulletClone;

    private Animator animator;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clip;

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
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audioSource.volume = 0.3f;

        audioSource.clip = clip[0];
        audioSource.Play();
        animator.Play("EnemyGunEmpect");
        bulletClone  = Instantiate(bulletPrefab,this.transform.position,this.transform.localRotation);
        //this.gameObject.SetActive(false);

    }

    public void OnDisable()
    {
        
    }
}
