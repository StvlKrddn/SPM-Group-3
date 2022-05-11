using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public GameObject UI;
    public GameObject position;
    public Color hoverColor;
    private Renderer rend;
    private Color startColor;
    public bool clicked = false;
    private Transform _selection;

    private bool hover = false;

    void Start()
    {
        BuilderController buildControl = FindObjectOfType<BuilderController>();
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void Update()
    {

    }

    public void SetDoNotHover()
    {
        hover = false;
    }

    public void Hover()
    {
        if (hover)
        {
            rend.material.color = hoverColor;
        }
    } 

    public void SetStartColor()
    {
        rend.material.color = startColor;
    }


}
