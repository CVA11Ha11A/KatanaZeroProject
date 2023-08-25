using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kissyface_Throw : MonoBehaviour
{
    public GameObject throwAxePrefab;
    public bool isReturn=false;
    private Transform targetTransform;
    Animator anim;
    Kissyface_manager manager;
    Vector3 leftAngle = new Vector3(0, 180, 0);
    Vector3 rightAngle = new Vector3(0, 0, 0);

    // Start is called before the first frame update

    private void OnEnable()
    {
        manager = GetComponent<Kissyface_manager>();
        anim = GetComponent<Animator>();
        anim.Play("Kissyface_throw");
        targetTransform = manager.playerTransform;
        if (targetTransform.position.x < transform.position.x && transform.eulerAngles != leftAngle)
        {
            transform.eulerAngles = leftAngle;
        }
        else if (targetTransform.position.x > transform.position.x && transform.eulerAngles != rightAngle)
        {
            transform.eulerAngles = rightAngle;
        }
        StartCoroutine(ThrowRoutine());
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private IEnumerator ThrowRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        manager.isAttackable = true;
        anim.Play("Kissyface_tug");
        GameObject axe = Instantiate(throwAxePrefab, transform.position, transform.rotation);

    }
    public IEnumerator ReturnRoutine()
    {
        anim.Play("kissyface_returnaxe");
        manager.isAction = false;
        yield return new WaitForSeconds(1f);

    }
}
