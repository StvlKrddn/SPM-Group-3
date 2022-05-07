using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public GameObject UI;
    public GameObject position;
/*    public GameObject placementClicked;*/
    public Color hoverColor;
    public GameManager gameManager;
    private Renderer rend;
    private Color startColor;
    public bool clicked = false;
    /*    private GameObject placedTower;*/
    private Transform _selection;

    BuildManager buildManager;
    private bool hover = false;

    // Start is called before the first frame update
    void Start()
    {
        BuilderController buildControl = FindObjectOfType<BuilderController>();
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void SetAreaClicked()
    {
        if (buildManager.ClickedArea != null)
        {
            Debug.Log("Tower Already There = 100");
            return;
        }
        else
        {
            Debug.Log("weeee");
            buildManager.ClickedArea = gameObject;
            clicked = true;
        }
    }

    public void SetDoNotHover()
    {
        hover = false;
    }

    public void HoverEffect(RaycastHit hit)
    {

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
