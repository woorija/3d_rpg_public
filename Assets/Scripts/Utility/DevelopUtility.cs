using UnityEngine;

public static class DevelopUtility
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object _log)
    {
        Debug.Log(_log);
    }
}
