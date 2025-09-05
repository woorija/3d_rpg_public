using System.Collections.Generic;
using UnityEngine;

public class BT_RandomSelectorNode : BT_CompositeNode
{
    [SerializeField] protected int[] randomPercentage;
    private HashSet<int> exceptNumber = new HashSet<int>();
    private int totalSum;

    protected override void Awake()
    {
        base.Awake();
        totalSum = 0;
        for (int i = 0; i < randomPercentage.Length; i++)
        {
            totalSum += randomPercentage[i];
        }
    }

    public override BTResult Execute()
    {
        if (children?.Count == 0) return BTResult.Failure;

        exceptNumber.Clear();
        int remainingSum = totalSum;

        int childrenCount = children.Count;
        for (int i = 0; i < childrenCount; i++)
        {
            int randomValue = Random.Range(0, remainingSum);
            int executeIndex = GetRandomIndex(randomValue, ref remainingSum);

            BTResult result = children[executeIndex].Execute();

            if (result != BTResult.Failure) return result;
            exceptNumber.Add(executeIndex);
        }
        return BTResult.Failure;
    }

    private int GetRandomIndex(int randomValue, ref int remainingSum)
    {
        int sum = 0;
        int length = randomPercentage.Length;
        for (int i = 0; i < length; i++)
        {
            if (!exceptNumber.Contains(i))
            {
                sum += randomPercentage[i];
                if (randomValue < sum)
                {
                    remainingSum -= randomPercentage[i];
                    return i;
                }
            }
        }
        return 0;
    }
}