using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    //각 몬스터의 BT는 해당 클래스를 베이스로 상속받음
    [SerializeField] protected BT_Node RootNode;
    [SerializeField] protected Animator animator; // 몬스터의 애니메이터

    [SerializeField] protected BT_Node RunningNode; // 러닝상태인 액션노드가 있을때 루트노드 대신 동작할 노드
    [SerializeField] protected BaseBlackBoard blackBoard;
    protected int currentAnimationHash;
    //블랙보드는 각 몬스터 BT내에서 선언할것
    public virtual BaseBlackBoard GetBlackBoard() { return blackBoard; }     // 블랙보드를 가져오는 함수
    public virtual void GetRootNode(BT_Node _node)
    {
        RootNode = _node;
    }
    public virtual void GetRunningNode(BT_Node _node) // 러닝상태의 노드를 가져오는 함수
    {
        if (RunningNode == _node) return;
        if (RunningNode != null)
        {
            if (RunningNode.Priority >= _node.Priority) return;
        }
        RunningNode = _node;
    }
    public virtual void CheckDeleteRunningNode(int _priority) // 러닝상태의 노드를 제거하는 함수
    {
        if (RunningNode != null)
        {
            if (_priority > RunningNode.Priority)
            {
                RunningNode = null;
            }
        }
    }
    public void ChangeAnimation(int _hash, int _layer = 0, float _normalizedTime = 0f)
    {
        if(currentAnimationHash == _hash) return;
        animator.Play(_hash, _layer, _normalizedTime);
        currentAnimationHash = _hash;
    }
    public void ChangeAnimationCrossFade(int _hash, float _crossFadeTime, int _layer = 0, float _normalizedTime = 0f)
    {
        if (currentAnimationHash == _hash) return;
        animator.CrossFade(_hash, _crossFadeTime, _layer, _normalizedTime);
        currentAnimationHash = _hash;
    }
    public void ChangeAnimatorBool(int _hash, bool _value)
    {
        if (animator.GetBool(_hash) == _value) return;
        animator.SetBool(_hash, _value);
    }
    public void ChangeAnimatorInt(int _hash, int _value)
    {
        if (animator.GetInteger(_hash) == _value) return;
        animator.SetInteger(_hash, _value);
    }
    public void ChangeAnimatorTrigger(int _hash)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == _hash) return;
        animator.SetTrigger(_hash);
    }
    public bool IsAnimationEnd(int _hash)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != _hash) return false;
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f;
    }
    public bool IsCurrentAnimatorStateName(int _hash)
    {
        return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == _hash;
    }
    public float GetCurrentAnimationTime(int _hash)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != _hash) return -1f;
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
    }
    public void PauseAnimation()
    {
        animator.speed = 0f;
    }
    public void PlayAnimation()
    {
        animator.speed = 1f;
    }
    public void ReplayAnimation()
    {
        PlayAnimation();
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0);
    }
    public Animator GetAnimator()
    {
        return animator;
    }
    public virtual void MeshSetActiveFalse()
    {
        animator.gameObject.SetActive(false);
    }
    public virtual void MeshSetActiveTrue()
    {
        animator.gameObject.SetActive(true);
    }
}
