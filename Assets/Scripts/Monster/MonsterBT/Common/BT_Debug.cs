using UnityEngine;

public class BT_Debug : BT_ActionNode
{
    [SerializeField] string message;
    public override BTResult Execute()
    {
        Debug.Log(message);
        return BTResult.Success;
    }
}
