using System.Collections.Generic;

public class SfxComparer : IComparer<SfxData>
{
    public int Compare(SfxData x, SfxData y)
    {
        return x.priority.CompareTo(y.priority);
    }
}
