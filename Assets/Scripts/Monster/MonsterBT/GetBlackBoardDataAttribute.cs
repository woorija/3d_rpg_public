using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class GetBlackBoardDataAttribute : PropertyAttribute
{
    public System.Type valueType;
    public GetBlackBoardDataAttribute(System.Type _type)
    {
        valueType = _type;
    }
}
