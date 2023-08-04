using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SG_CameraMove : MonoBehaviour
{


    public Transform target;
    public Vector2 offset;
    public Vector2 minPosition;
    public Vector2 maxPosition;


    //public Transform target; // 플레이어의 Transform 컴포넌트를 여기에 할당합니다.
    //public Vector2 offset; // 카메라와 플레이어 사이의 거리를 조정하는 오프셋 값입니다.

    void Awake()
    {
        //this.transform.position = new Vector3(0f, 0.75f, -10f);

        offset = new Vector2(0f, 0.75f);
        minPosition = new Vector2(-1.5f, -2f);
        maxPosition = new Vector2(105f, 30f);

    }

    public void Start()
    {
        
    }

    public void Update()
    {

    }





    void LateUpdate()
    {
        if (target == null)
            return;

        // 플레이어의 현재 위치에 오프셋을 더한 위치로 카메라를 이동시킵니다.
        Vector3 targetPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

    

        // 카메라의 위치를 지정된 범위 내에서 제한합니다.
        float clampedX = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

        // 제한된 값을 카메라 위치로 설정합니다.
        
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);


    }
}


