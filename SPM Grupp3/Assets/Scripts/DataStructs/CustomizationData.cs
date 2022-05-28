using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CustomizationData
{
    public SerializableColor player1Color, player2Color;

    public CustomizationData(Color player1Color, Color player2Color)
    {
        this.player1Color = player1Color;
        this.player2Color = player2Color;
    }
}
