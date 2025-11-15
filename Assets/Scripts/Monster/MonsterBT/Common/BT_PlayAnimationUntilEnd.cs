using UnityEngine;

public class BT_PlayAnimationUntilEnd : BT_ActionNode
{
    [field: SerializeField, HideInInspector] public int animationStateHash { get; protected set; }
    [field: SerializeField, HideInInspector] public int exitPriority {  get; protected set; }
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd(animationStateHash))
        {
            BT.CheckDeleteRunningNode(exitPriority);
            return BTResult.Success;
        }
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
#if UNITY_EDITOR
    public void SetHash(int _hash)
    {
        animationStateHash = _hash;
    }
    public void SetPriority(int _priority)
    {
        exitPriority = _priority;
    }
#endif
}
