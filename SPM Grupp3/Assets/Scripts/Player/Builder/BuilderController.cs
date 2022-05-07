using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

/*
 * This class is based on this YouTube tutorial: https://youtu.be/Y3WNwl1ObC8
 */

public class BuilderController : MonoBehaviour
{
    [SerializeField] private float cursorSpeed = 1000f;
    [Space]
    [SerializeField] private GameObject cursorPrefab;
    [SerializeField] private LayerMask placeForTowerLayerMask;
    [SerializeField] private LayerMask towerLayerMask;
    [SerializeField] private LayerMask ghostTower;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color startColor;
    [SerializeField] private Color towerPreview;
    private Transform _selection;

    private Camera mainCamera;
    private Mouse virtualMouse;
    private RectTransform cursorTransform;
    private Transform canvas;
    private PlayerInput playerInput;
    private InputAction pointerAction;
    private InputAction enterTankAction;
    private Vector2 newPosition;
    private Vector2 screenMiddle;
    private bool previousMouseState;
    private bool previousYState;
    private GameObject preTower;
    private bool clicked = false;
    private GameObject infoView;
    private GameObject medium;

    void Start()
    {
        mainCamera = Camera.main;
        canvas = mainCamera.transform.Find("CanvasV2");
        infoView = GameObject.Find("InfoView");
        medium = GameObject.Find("Medium");

        screenMiddle = new Vector2(Screen.width / 2, Screen.height / 2);

        EventHandler.Instance.RegisterListener<EnterBuildModeEvent>(OnEnterBuildMode);
    }
    
    private void OnEnterBuildMode(EnterBuildModeEvent eventInfo)
    {
        InitializeInputSystem(eventInfo.Player);
        InitializeCursor();
        InitializeVirtualMouse();
        ResetPosition();
    }

    void InitializeInputSystem(GameObject player)
    {
        playerInput = player.GetComponent<PlayerInput>();
        pointerAction = playerInput.actions["LeftStick"];
    }

    void InitializeVirtualMouse()
    {
        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }

        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }
    }

    void InitializeCursor()
    {
        GameObject cursor = Instantiate(cursorPrefab, canvas);
        SetCursorColor(cursor);
        cursor.name = "Player " + (playerInput.playerIndex + 1) + " cursor";
        cursorTransform = cursor.GetComponent<RectTransform>();
        cursorTransform.gameObject.SetActive(true);
    }
    

    void SetCursorColor(GameObject cursor)
    {
        Image cursorImage = cursor.GetComponent<Image>();
        cursorImage.color = playerInput.playerIndex == 0 ? Color.blue : Color.red;
    }

    public void AcceptAction (InputAction.CallbackContext context)
    {
        bool isPressed = context.performed;
        virtualMouse.CopyState(out MouseState mouseState);
        mouseState.WithButton(MouseButton.Left, isPressed);
        InputState.Change(virtualMouse, mouseState);
        if (isPressed)
        {
            if (!clicked)
            {
                ClickedPlacement();
                ClickedTower();
                clicked = true;
                Invoke(nameof(ClickDelay), 0.1f);
            }

        }
        previousMouseState = isPressed;
    }
    
    private void ClickDelay()
    {
        clicked = false;
    }

    public void InfoAction (InputAction.CallbackContext context)
    {
        bool isPressed = context.performed;
        virtualMouse.CopyState(out MouseState mouseState);
        mouseState.WithButton(MouseButton.Left, isPressed);
        InputState.Change(virtualMouse, mouseState);
        previousYState = isPressed;
    }

    public void EnterTank(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            medium.SetActive(true);

            BuildManager.instance.TowerToBuild = null;
            Destroy(preTower);
            Renderer selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material.color = startColor;
            ResetPosition();
            cursorTransform.gameObject.SetActive(false);
            EventHandler.Instance.InvokeEvent(new PlayerSwitchEvent(
                description: "Player switched mode",
                playerContainer: transform.parent.gameObject
            ));
        }
    }

    private void OnEnable()
    {
        InputSystem.onAfterUpdate += UpdateVirtualMouse;
    }


    void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateVirtualMouse;
    }

    void OnDestroy()
    {
        InputSystem.RemoveDevice(InputSystem.GetDevice("VirtualMouse"));
    }

    void ResetPosition()
    {
        InputState.Change(virtualMouse.position, screenMiddle);
        UpdateCursorImage(screenMiddle);
    }

    void UpdateVirtualMouse()
    {
        if (virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        //Denna borde antagligen flyttas
        if (Gamepad.current.bButton.isPressed)
        {
            BuildManager.instance.TowerToBuild = null;
            Destroy(preTower);
            Renderer selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material.color = startColor;
            medium.SetActive(true);
            
            for (int i = 0; i < infoView.transform.childCount; i++)
            {
                infoView.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        newPosition = MoveMouse();
        UpdateCursorImage(newPosition);

        RaycastHit hit = CastRayFromCamera(placeForTowerLayerMask);
        Hover(hit);
    }


    Vector2 MoveMouse()
    {
        Vector2 cursorMovement = pointerAction.ReadValue<Vector2>();
        cursorMovement *= cursorSpeed * Time.unscaledDeltaTime;
        Vector2 newPosition = virtualMouse.position.ReadValue() + cursorMovement;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, cursorMovement);

        return newPosition;
    }
    
    void UpdateCursorImage(Vector2 newPosition)
    {
        if (cursorTransform != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect: canvas.GetComponent<RectTransform>(), 
                screenPoint: newPosition,
                cam: canvas.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, 
                localPoint: out Vector2 anchoredPosition
                );
            cursorTransform.anchoredPosition = anchoredPosition;
        }
    }
    RaycastHit CastRayFromCamera(LayerMask layerMask)
    {
        // Get mouse position
        Vector3 mousePosition = newPosition;

        // Create a ray from camera to mouse position
        Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray: cameraRay, hitInfo: out RaycastHit hit, maxDistance: Mathf.Infinity, layerMask: layerMask);
        
        return hit;
    }

    void Hover(RaycastHit hit)
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material.color = startColor;
            _selection = null;
            Destroy(preTower);
        }

        // Raycast along the ray and return the hit point
        if (hit.collider != null)
        {
            var selection = hit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material.color = hoverColor;

                BuildManager buildManager = BuildManager.instance;
                if (buildManager.TowerToBuild != null)
                {
                    RaycastHit hitTower = CastRayFromCamera(towerLayerMask);
                    if (hitTower.collider == null)
                    {
                        GhostTower(selection, buildManager);
                    }
                }
            }
            _selection = selection;
        }
    }

    void GhostTower(Transform selection, BuildManager buildManager)
    {
        int index = 1;
        for (;  index < buildManager.TowerToBuild.transform.childCount; index++)
        {
            bool active = buildManager.TowerToBuild.transform.GetChild(index).gameObject.activeSelf;
            if (active)
            {
                break;
            }
        }

        GameObject tower = buildManager.TowerToBuild.transform.GetChild(index).gameObject;
        
        Transform placement = selection.GetChild(0).transform;
        Vector3 placeVec = placement.position;
        Vector3 towerPlace = new Vector3(placeVec.x, placeVec.y + 0.5f, placeVec.z);

/*        Tower tow = tower.GetComponent<Tower>();
        GameObject radius = tower.transform.Find("Radius").gameObject;
        radius.transform.localScale = new Vector3(tow.range * 2f, 0.01f, tow.range * 2f);*/

        preTower = Instantiate(tower, towerPlace, placement.rotation);
        preTower.layer = 12;
        preTower.GetComponent<Renderer>().material.color = towerPreview;                        
    }

    GameObject GetTowerPlacement()
    {
        RaycastHit hit = CastRayFromCamera(placeForTowerLayerMask);
        return hit.collider != null ? hit.collider.gameObject : null;
    }

    void ClickedPlacement()
    {   
        RaycastHit hit = CastRayFromCamera(placeForTowerLayerMask);
        if (hit.collider != null)
        {
            GameObject placementHit = hit.collider.gameObject;
            if (placementHit.CompareTag("PlaceForTower"))
            {
                BuildManager buildManager = BuildManager.instance;
                if (buildManager.TowerToBuild != null)
                {
                    RaycastHit hitTower = CastRayFromCamera(towerLayerMask);
                    if (hitTower.collider == null)
                    {
                        buildManager.ClickedArea = _selection.gameObject;
                        buildManager.InstantiateTower();
                    }

                }
            }
        }
    }
    void ClickedTower()
    {
        RaycastHit hit = CastRayFromCamera(towerLayerMask);
        if (hit.collider != null)
        {
            GameObject towerHit = hit.collider.gameObject;
            if (towerHit.CompareTag("Tower"))
            {
                if (towerHit != null && preTower == null)
                {
                    Tower tower = towerHit.GetComponent<Tower>();
                    
                    tower.ShowUpgradeUI(medium, infoView);
                }
                

/*                if (tower.radius.activeInHierarchy)
                {
                    tower.radius.SetActive(false);
                    tower.upgradeUI.SetActive(false);
                        
                }
                else
                {
                    tower.radius.SetActive(true);
                    tower.upgradeUI.SetActive(true);

                } */
            }
        }
    }
}
