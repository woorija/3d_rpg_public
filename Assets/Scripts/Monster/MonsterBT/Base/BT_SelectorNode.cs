using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_SelectorNode : BT_CompositeNode
{
    public override BTResult Execute()
    {
        if (children == null || children.Count == 0) return BTResult.Failure;

        foreach(var child in children)
        {
            switch (child.Execute())
            {
                case BTResult.Running:
                    return BTResult.Running;
                case BTResult.Success:
                    return BTResult.Success;
            }
        }
        return BTResult.Failure;
    }
}
