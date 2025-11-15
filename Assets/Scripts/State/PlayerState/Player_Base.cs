using UnityEngine;
public class Player_Base : BaseState
{
    protected PlayerStatus status;
    protected PlayerController controller;
    [field: SerializeField, HideInInspector] public int animationStateHash {  get; protected set; }

    public override void Awake()
    {
        base.Awake();
        status = GetComponentInParent<PlayerStatus>();
        controller = GetComponentInParent<PlayerController>();
    }
    public override void StateEnter()
    {
        base.StateEnter();
        animator.Play(animationStateHash);
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
    }
    public override void StateExit()
    {
        base.StateExit();
    }
    protected float GetAnimationNormalizedTime()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
    }
#if UNITY_EDITOR
    public void SetHash(int _hash)
    {
        animationStateHash = _hash;
    }
#endif
}
