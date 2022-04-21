using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public GameObject UI;
    public GameObject position;
    public GameObject upgradeUI;
/*    public GameObject placementClicked;*/
    public Color hoverColor;
    public GameManager gameManager;
    private Renderer rend;
    private Color startColor;
    public bool clicked = false;
/*    private GameObject placedTower;*/
    

    BuildManager buildManager;
    public Shop shop;
    private Shop test;
    private bool hover = true;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
        test = UI.transform.GetChild(0).gameObject.GetComponent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (buildManager.ClickedArea != null)
        {
            upgradeUI.SetActive(true);
            Debug.Log("Tower Already There = 100");
            return;
        }
        else
        {
            buildManager.ClickedArea = gameObject;
            clicked = true;
        }


        /*InstantiateTower();*/

/*        if (UI.activeSelf)
        {
            buildManager.ClickedArea = null;
            UI.SetActive(false);
            clicked = false;
        }
        else
        {
            buildManager.ClickedArea = gameObject;
            UI.SetActive(true);
            clicked = true;
        }*/
    }

    public void SetDoNotHover()
    {
        hover = false;
    }

    private void OnMouseEnter()
    {
        if (hover)
        {
            rend.material.color = hoverColor;
        }
          
    }
    private void OnMouseExit()
    {
        if (clicked != true)
        {
            rend.material.color = startColor;
        }
        
    }

    public void SetStartColor()
    {
        rend.material.color = startColor;
    }


}
