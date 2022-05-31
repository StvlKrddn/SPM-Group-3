using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CustomizationData
{
    public SerializableColor PlayerColor;
    public bool HasHat;

    public CustomizationData(Color player1Color, bool hasHat)
    {
        PlayerColor = player1Color;
        HasHat = hasHat;
    }
}
