using System;
using UnityEngine;

[Serializable]
public struct CustomizationData
{
    public SerializableColor PlayerColor;
    public int PlayerClass;

    public CustomizationData(Color player1Color, int playerClass)
    {
        PlayerColor = player1Color;
        PlayerClass = playerClass;
    }
}
