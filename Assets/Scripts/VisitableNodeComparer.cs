using System.Collections.Generic;

public class VisitableNodeComparer : IComparer<float>
{
    public int Compare(float x, float y)
    {
        float result = x.CompareTo(y);
        return result == 0 ? 1 : (int)result;
    }
}
