using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Since Unity doesn't flag Color as serializable, we
/// need to create our own version. This struct will automatically convert
/// between Color and SerializableColor
/// </summary>
[Serializable]
public struct SerializableColor
{
    public float r;
    public float g;
    public float b;
    public float a;
    
    public SerializableColor(float rR, float rG, float rB, float rA)
    {
        r = rR;
        g = rG;
        b = rB;
        a = rA;
    }
    
    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}, {3}]", r, g, b, a);
    }
    
    /// <summary>
    /// Automatic conversion from SerializableColor to Color
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Color(SerializableColor rValue)
    {
        return new Color(rValue.r, rValue.g, rValue.b, rValue.a);
    }
    
    /// <summary>
    /// Automatic conversion from Color to SerializableColor
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableColor(Color rValue)
    {
        return new SerializableColor(rValue.r, rValue.g, rValue.b, rValue.a);
    }
}
