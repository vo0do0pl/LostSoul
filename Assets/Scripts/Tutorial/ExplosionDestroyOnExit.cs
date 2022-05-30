using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroyOnExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Destroy(animator.gameObject, stateInfo.length);
    }
}
