using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayerLayerScript : StateMachineBehaviour
{

    bool firstRool = false;

    // 애니메이션 상태의 해시값을 저장할 변수
    private int idleStateHash;
    private int rollStateHash;
    private int runStateHash;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 새로운 상태로 변할 때 실행
        idleStateHash = Animator.StringToHash("Base Layer.Idle");
        rollStateHash = Animator.StringToHash("Base Layer.IsRoll");
        runStateHash = Animator.StringToHash("Base Layer.Run");

        if (stateInfo.fullPathHash == runStateHash)
        {
            //Debug.Log("뛰는게 끝날때 들어오긴하나?");
            animator.SetTrigger("ExitRun");
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 처음과 마지막 프레임을 제외한 각 프레임 단위로 실행
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 상태가 다음 상태로 바뀌기 직전에 실행

        // "Roll" 애니메이션 상태가 끝날 때 특정 작업을 수행합니다.
        if (stateInfo.fullPathHash == rollStateHash && firstRool == false)
        {
            firstRool = true;
            animator.Play("Run");
        }
      

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MonoBehaviour.OnAnimatorMove 직후에 실행
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MonoBehaviour.OnAnimatorIK 직후에 실행
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // 스크립트가 부착된 상태 기계로 전환이 왔을때 실행
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        // 스크립트가 부착된 상태 기계에서 빠져나올때 실행
    }

}
