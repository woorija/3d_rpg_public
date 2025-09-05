using UnityEngine;

public class SkillStateBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsPlayingSkill", true);
        animator.SetInteger("SkillId", 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsPlayingSkill", false);
    }
}
