using UnityEngine;

public class ControlHandling : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsHit", false);
    }
}
