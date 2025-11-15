using UnityEngine;

public class SkillStateBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimationKey.IsPlayingSkill, true);
        animator.SetInteger(AnimationKey.SkillId, 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimationKey.IsPlayingSkill, false);
    }
}
