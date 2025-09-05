using System.Collections.Generic;

public class QuestRewards
{
    public int exp { get; private set; }
    public int gold { get; private set; }
    public List<int> itemIds { get; private set; }
    public List<int> itemAmounts { get; private set; }

    public QuestRewards(int _exp, int _gold, List<int> _itemIds, List<int> _itemAmounts)
    {
        exp = _exp;
        gold = _gold;
        itemIds = _itemIds;
        itemAmounts = _itemAmounts;
    }
}
