using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    /// <summary> What kind of action is this </summary>
    [Tooltip("What kind of action is this")]
    [SerializeField] private ActionType actionType;

    /// <summary> Spherical radius of the actions effective range </summary>
    [Tooltip("Spherical radius of the actions effective range")]
    [SerializeField] private float range;

    /// <summary> How often is the action performed every second </summary>
    [Tooltip("How often is the action performed every second")]
    [SerializeField] private float frequency;

    /// <summary> Is this an area of effect or single target action </summary>
    [Tooltip("Is this an area of effect or single target action")]
    [SerializeField] private bool areaOfEffect;

    public ActionType ActionType { get { return actionType; } }
    public float Range { get { return range; } }
    public float Frequency { get { return frequency; } }
    public bool AreaOfEffect { get { return areaOfEffect; } }


    void Start()
    {

    }

    void Update()
    {

    }
}
