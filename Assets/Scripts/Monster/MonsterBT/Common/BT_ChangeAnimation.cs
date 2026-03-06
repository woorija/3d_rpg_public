using UnityEngine;

public class BT_ChangeAnimation : BT_ActionNode
{
    [field: SerializeField, HideInInspector] public int animationStateHash { get; protected set; }
    [field: SerializeField, HideInInspector] public bool useCrossFade { get; protected set; } = false;
    [field: SerializeField, HideInInspector, Range(0f, 1f)] public float crossFadeTime { get; protected set; } = 0.2f;

    public override BTResult Execute()
    {
        if (useCrossFade)
        {
            BT.ChangeAnimationCrossFade(animationStateHash, crossFadeTime);
        }
        else
        {
            BT.ChangeAnimation(animationStateHash);
        }
        BT.PlayAnimation();
        return BTResult.Success;
    }
#if UNITY_EDITOR
    public void SetHash(int _hash)
    {
        animationStateHash = _hash;
    }
    public void SetUseCrossFade(bool _useCrossFade)
    {
        useCrossFade = _useCrossFade;
    }
    public void SetCrossFadeTime(float _crossFadeTime)
    {
        crossFadeTime = _crossFadeTime;
    }
#endif
}
