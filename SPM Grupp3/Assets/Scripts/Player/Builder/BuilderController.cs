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
    [SerializeField] private LayerMask uiLayer;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color startColor;
    [SerializeField] private Color towerPreview;

    private Transform _selection;
    private BuildManager buildManager;
    private Camera mainCamera;
    private Mouse virtualMouse;
    private RectTransform cursorTransform;
    private Transform canvas;
    private PlayerInput playerInput;
    private InputAction pointerAction;
    private Vector2 newPosition;
    private Vector2 screenMiddle;
    private bool previousMouseState;
    private bool previousYState;
    private GameObject preTower;
    private GameObject buildMenu;
    private GameObject infoView;
    private GameObject towerPanel;
    private GameObject playerCursor;

    private GameObject towerMenu;
    private GameObject upgradeMenu;
    private bool placementClicked = false;

    void Start()
    {
        screenMiddle = new Vector2(Screen.width / 2, Screen.height / 2);

        mainCamera = Camera.main;
        canvas = UI.Canvas.transform;
        buildMenu = canvas.Find("Build_UI").gameObject;
        infoView = buildMenu.transform.Find("InfoViews").gameObject;
        towerPanel = buildMenu.transform.Find("TowerPanel").gameObject;

        towerMenu = canvas.Find("TowerMenu").GetChild(0).gameObject;
        upgradeMenu = canvas.Find("UpgradeMenu").GetChild(0).gameObject;

        buildManager = GetComponentInParent<BuildManager>();

        InitializeInputSystem();
        InitializeCursor();
        InitializeVirtualMouse();

        ResetCursorPosition();
    }

    void InitializeInputSystem()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        pointerAction = playerInput.actions["LeftStick"];
    }

    void InitializeCursor()
    {
        playerCursor = Instantiate(cursorPrefab, transform.parent.Find("PlayerCanvas"));
        SetCursorColor(playerCursor);
        playerCursor.name = "Player " + (playerInput.playerIndex + 1) + " cursor";
        cursorTransform = playerCursor.GetComponent<RectTransform>();
        cursorTransform.gameObject.SetActive(true);
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
        CursorHandler cursor = playerCursor.GetComponent<CursorHandler>();
        cursor.ToggleClick(isPressed);
        if (isPressed)
        {
            ClickedPlacement();
            ClickedTower();
            EventHandler.Instance.InvokeEvent(new UIClickedEvent(
                description: "Accept button clicked",
                clicker: transform.parent.gameObject
            ));
        }
        previousMouseState = isPressed;
    }

    public void BackAction (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Deselect();
            for (int i = 0; i < infoView.transform.childCount; i++)
            {
                infoView.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void EnterTank(InputAction.CallbackContext context)
    {
        if (context.performed )
        {
            Deselect();
            EventHandler.Instance.InvokeEvent(new PlayerSwitchEvent(
                description: "Player switched mode",
                playerContainer: transform.parent.gameObject
            ));
        }
    }

    private void Deselect()
    {
        buildManager.TowerToBuild = null;
        Destroy(preTower);
        if (_selection != null)
        {
            Renderer selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material.color = startColor;
        }
        towerMenu.SetActive(false);
        placementClicked = false;
        towerPanel.SetActive(true);
    }

    private void OnEnable()
    {
        InputSystem.onAfterUpdate += UpdateVirtualMouse;
        if (cursorTransform != null)
        {
            ResetCursorPosition();
            cursorTransform.gameObject.SetActive(true);            
        }
    }

    void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateVirtualMouse;
        if (cursorTransform != null)
        {
            ResetCursorPosition();
            cursorTransform.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (virtualMouse != null)
        {
            InputSystem.RemoveDevice(InputSystem.GetDevice("VirtualMouse"));
        }
    }

    void ResetCursorPosition()
    {
        InputState.Change(virtualMouse.position, screenMiddle);
        UpdateCursorImage(screenMiddle);
    }

    void UpdateVirtualMouse()
    {
        if (virtualMouse == null)
        {
            return;
        }

        newPosition = MoveMouse();
        UpdateCursorImage(newPosition);

        RaycastHit hit = CastRayFromCamera(placeForTowerLayerMask);

        if (!placementClicked)
        {
            Hover(hit);
        }
       
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
        }

        // Raycast along the ray and return the hit point
        if (hit.collider != null)
        {
            var selection = hit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material.color = hoverColor;
            }
            _selection = selection;
        }
    }

    public void ResetHover()
    {
        placementClicked = false;
    }

    public void ExitHover()
    {
        buildManager.TowerToBuild = null;
        GhostTower();
    }

    public void TowerToHover(GameObject tower)
    {
        buildManager.TowerToBuild = tower;
        GhostTower();
    }

    void GhostTower()
    {
        if (buildManager.ClickedArea == null || buildManager.TowerToBuild == null)
        {
            Destroy(preTower);
            return;
        }

        if (GameManager.Instance.CheckIfEnoughResourcesForTower())
        {
            GameObject tower = buildManager.TowerToBuild.transform.GetChild(1).gameObject;


            Transform placement = buildManager.ClickedArea.transform.GetChild(0).transform;
            Vector3 placeVec = placement.position;
            Vector3 towerPlace = new Vector3(placeVec.x, placeVec.y + 0.5f, placeVec.z);

            preTower = Instantiate(tower, towerPlace, placement.rotation);
            GameObject radius = preTower.transform.GetChild(0).gameObject;

            preTower.layer = 12;
            preTower.GetComponent<Renderer>().material.color = towerPreview;

            Tower tow = buildManager.TowerToBuild.GetComponent<Tower>();
            radius.transform.localScale = new Vector3(tow.range * 2f, 0.01f, tow.range * 2f);
            radius.SetActive(true);
        }       
    }

    GameObject GetTowerPlacement()
    {
        RaycastHit hit = CastRayFromCamera(placeForTowerLayerMask);
        return hit.collider != null ? hit.collider.gameObject : null;
    }

    void ClickedPlacement()
    {   
        if (!placementClicked)
        {
            RaycastHit hit = CastRayFromCamera(placeForTowerLayerMask);
            if (hit.collider != null)
            {
                GameObject placementHit = hit.collider.gameObject;
                if (placementHit.CompareTag("PlaceForTower"))
                {
                    buildManager.ClickedArea = _selection.gameObject;
                    _selection.gameObject.GetComponent<Renderer>().material.color = hoverColor;
                    placementClicked = true;
                    towerMenu.SetActive(true);
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

            if (towerHit != null && preTower == null)
            {
                Tower tower = towerHit.GetComponent<Tower>();
                    
                tower.ShowUpgradeUI(upgradeMenu);
                EventHandler.Instance.InvokeEvent(new TowerClickedEvent("Tower Is clicked", tower.gameObject));
                buildManager.TowerToBuild = null;
                print("Tower of type: " + tower + " Is clicked");
                placementClicked = true;
            }

        }

    }
}
