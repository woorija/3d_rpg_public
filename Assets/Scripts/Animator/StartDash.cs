using UnityEngine;

public class StartDash : StateMachineBehaviour
{
    BaseBlackBoard blackBoard;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(blackBoard == null)
        {
            blackBoard = animator.GetComponentInParent<BaseBlackBoard>();
        }

        blackBoard.StartDash();
    }
}
