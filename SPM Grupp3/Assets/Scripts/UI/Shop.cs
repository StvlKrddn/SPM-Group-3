using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private Button pressedButton;
    private BuildManager buildManager;

    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        Camera canvasCamera = GameObject.FindGameObjectWithTag("Camera/CanvasCamera").GetComponent<Camera>();
        canvas.worldCamera = canvasCamera;
    }

    public void PurchaseCannonTower()
    {
        buildManager = GetComponentInParent<BuildManager>();
        buildManager.TowerToBuild = buildManager.CannonTowerPrefab;
        buildManager.InstantiateTower();
    }

    public void PurchaseMissileTower()
    {
        buildManager = GetComponentInParent<BuildManager>();
        buildManager.TowerToBuild = buildManager.MissileTowerPrefab;
        buildManager.InstantiateTower();
    }

    public void PurchaseSlowTower()
    {
        buildManager = GetComponentInParent<BuildManager>();
        buildManager.TowerToBuild = buildManager.SlowTowerPrefab;
        buildManager.InstantiateTower();
    }

    public void PurchasePoisonTower()
    {
        buildManager = GetComponentInParent<BuildManager>();
        buildManager.TowerToBuild = buildManager.PoisonTowerPrefab;
        buildManager.InstantiateTower();
    }

    public void OnClicked(Button button)
    {
        pressedButton = button;
        PurchaseTower();
    }

    private void PurchaseTower()
    {
        switch (pressedButton.name)
        {
            case "Cannon":
                PurchaseCannonTower();
                break;
            case "Missile":
                PurchaseMissileTower();
                break;
            case "Poison":
                PurchasePoisonTower();
                break;
            case "Slow":
                PurchaseSlowTower();
                break;
        }
    }
}
