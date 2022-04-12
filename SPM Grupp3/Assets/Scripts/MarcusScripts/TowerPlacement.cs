using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public GameObject UI;
    public GameObject position;
    private GameObject placedTower;
/*    public GameObject placementClicked;*/
    public Color hoverColor;
    private Renderer rend;
    private Color startColor;
    private bool clicked = false;

    BuildManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (placedTower != null)
        {
            Debug.Log("Tower Already There");
            return;
        }

        if (buildManager.TowerToBuild == null)
        {
            return;
        }

        InstantiateTower();

/*        if (UI.activeSelf)
        {
            placementClicked = null;
            UI.SetActive(false);
            clicked = false;
        }
        else
        {
            placementClicked = gameObject;
            UI.SetActive(true);
            clicked = true;
        }*/
    }



    private void OnMouseEnter()
    {
        if (placedTower == null)
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

    public void InstantiateTower()
    {
        GameObject towerToBuild = buildManager.TowerToBuild;
        placedTower = Instantiate(towerToBuild, position.transform.position, position.transform.rotation);        
        /*UI.SetActive(false);*/
    }
}
