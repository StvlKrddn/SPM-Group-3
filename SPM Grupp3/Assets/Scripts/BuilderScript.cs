using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderScript : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask gameBoardMask;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private GameObject tower;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTower();
        }
        if (Input.GetMouseButton(1))
        {
            DeleteTower();
        }
    }

    void DeleteTower()
    {
        GameObject tower = GetTower();
        if (tower != null)
        {
            Destroy(tower);
        }
    }

    GameObject GetTower()
    {
        RaycastHit hit = CastRayFromCamera(towerMask);

        // If a tower was hit, return the tower
        return hit.collider != null ? hit.collider.gameObject : null;
    }

    void SpawnTower()
    {
        Vector3 boardPosition = GetBoardPosition();
        GameObject newTower = Instantiate(original: tower, position: boardPosition, rotation: tower.transform.rotation);
    }


    Vector3 GetBoardPosition()
    {
        RaycastHit hit = CastRayFromCamera(gameBoardMask);
        
        // Return a rounded position on the grid
        return hit.collider != null ? new Vector3(Mathf.Round(hit.point.x), Mathf.Round(hit.point.y), Mathf.Round(hit.point.z)) : Vector3.zero;
    }

    RaycastHit CastRayFromCamera(LayerMask layerMask)
    {
        // Get mouse position
        Vector3 mousePosition = Input.mousePosition;

        // Create a ray from camera to mouse position
        Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);

        // Raycast along the ray and return the hit point
        Physics.Raycast(ray: cameraRay, hitInfo: out RaycastHit hit, maxDistance: Mathf.Infinity, layerMask: layerMask);
        
        return hit;
    }
}
