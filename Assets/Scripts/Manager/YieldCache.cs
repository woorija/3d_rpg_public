using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class YieldCache
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y) { return x == y; }
        int IEqualityComparer<float>.GetHashCode(float obj) { return obj.GetHashCode();}
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    public static readonly Dictionary<float,WaitForSeconds> _timeInterval = new Dictionary<float,WaitForSeconds>(new FloatComparer());
    public static readonly Dictionary<float,WaitForSecondsRealtime> _realtimeInterval = new Dictionary<float,WaitForSecondsRealtime>(new FloatComparer());
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if(!_timeInterval.TryGetValue(seconds,out wfs))
        {
            _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        }
        return wfs;
    }
    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        WaitForSecondsRealtime wfs;
        if (!_realtimeInterval.TryGetValue(seconds, out wfs))
        {
            _realtimeInterval.Add(seconds, wfs = new WaitForSecondsRealtime(seconds));
        }
        return wfs;
    }
}