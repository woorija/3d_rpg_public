using System;
using UnityEngine;

public class InterfacesAttribute : PropertyAttribute
{
    public Type InterfaceType { get; private set; }

    public InterfacesAttribute(Type interfaceType)
    {
        InterfaceType = interfaceType;
    }
}