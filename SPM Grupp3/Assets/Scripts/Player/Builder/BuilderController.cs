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
    [SerializeField] private LayerMask garageLayer;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color towerPreview;
    private Color player1Color;
    private Color player2Color;

    private Color startColor;
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
    private GameObject playerCursor;
    private Tower selectedTower;
    private Transform playerUI;
    private Transform towerMenu;
    private GameObject buildPanel;
    private GameObject hintsPanel;
    private GameObject tankUpgrade;

    private GameObject towerHit;
    private GameObject placementHit;
    private GameObject garageHit;

    private bool stopHover = false;
    private bool stopMouse = false;
    private bool placementClicked = false;
    private bool purchasedInUI = false;

    void Start()
    {
        GameObject placement = GameObject.Find("PlaceForTower").gameObject;
        startColor = placement.GetComponent<Renderer>().material.color;
        screenMiddle = new Vector2(Screen.width / 2, Screen.height / 2);

        player1Color = GameManager.Instance.Player1Color;
        player2Color = GameManager.Instance.Player2Color;

        mainCamera = Camera.main;
        canvas = UI.Canvas.transform;

        playerUI = transform.parent.Find("PlayerUI");
        towerMenu = playerUI.Find("TowerMenu");
        buildPanel = towerMenu.Find("BuildPanel").gameObject;
        hintsPanel = towerMenu.Find("Hints").gameObject;
        tankUpgrade = towerMenu.Find("TankPanel").gameObject;

        buildManager = GetComponentInParent<BuildManager>();
        EventHandler.Instance.RegisterListener<BoughtInUIEvent>(SetBoughtInUI);

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
        playerCursor = Instantiate(cursorPrefab, playerUI);
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
        cursorImage.color = playerInput.playerIndex == 0 ? player1Color : player2Color;


		if (playerInput.playerIndex == 0)
		{
			for (int i = 0; i < towerMenu.transform.childCount; i++)
			{
				if (towerMenu.transform.GetChild(i).name.Equals("Hints"))
				{
					continue;
				}
				towerMenu.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = player1Color;
			}

		}

		if (playerInput.playerIndex == 1)
		{
			for (int i = 0; i < towerMenu.transform.childCount; i++)
			{
				if (towerMenu.transform.GetChild(i).name.Equals("Hints"))
				{
					continue;
				}
				towerMenu.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = player2Color;
			}

		}
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
            ClickedGarage();
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
            
        }
    }

    public void EnterTank(InputAction.CallbackContext context)
    {
        if (context.performed && !stopMouse)
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

        for (int i = 0; i < towerMenu.childCount; i++)
        {
            towerMenu.GetChild(i).gameObject.SetActive(false);
        }

        if (towerHit != null && placementHit.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            placementHit.layer = LayerMask.NameToLayer("PlaceForTower");
        }
        else if (towerHit != null && towerHit.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            towerHit.layer = LayerMask.NameToLayer("Towers");
        }
        else if (garageHit.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            garageHit.layer = LayerMask.NameToLayer("Garage");
        }
        
        hintsPanel.SetActive(false);
        cursorTransform.gameObject.SetActive(true);
        stopHover = false;
        stopMouse = false;
        placementClicked = false;
/*        towerPanel.SetActive(true);*/
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
        if (!stopMouse)
        {
            if (purchasedInUI)
            {
                if (pointerAction.ReadValue<Vector2>().x > 0.2f || pointerAction.ReadValue<Vector2>().y > 0.2f)
                {
                    return;
                }
                else
                {
                    newPosition = MoveMouse();
                    UpdateCursorImage(newPosition);
                    purchasedInUI = false;
                }
            }
            else
            {
                newPosition = MoveMouse();
                UpdateCursorImage(newPosition);
            }
            
        }

        RaycastHit hit = CastRayFromCamera(placeForTowerLayerMask);

        if (!stopHover)
        {
            Hover(hit);
        }        
    }

    void SetBoughtInUI(BoughtInUIEvent eventInfo)
    {
        purchasedInUI = true;
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
        Deselect();
        stopHover = false;
        stopMouse = false;
        placementClicked = false;
        buildManager.TowerToBuild = null;
        buildManager.ClickedArea = null;
        cursorTransform.gameObject.SetActive(true);
        GhostTower(buildManager.TowerToBuild);
    }

/*    public void ExitHover()
    {
        buildManager.TowerToBuild = null;
        GhostTower();
    }

    public void TowerToHover(GameObject tower)
    {
        buildManager.TowerToBuild = tower;
        GhostTower();
    }*/

    public void GhostTower(GameObject towerToDisplay)
    {
        if (towerToDisplay == null || buildManager.ClickedArea == null)
        {
            Destroy(preTower);
            return;
        }
        GameObject tower = towerToDisplay.transform.GetChild(1).gameObject;
        
        Transform placement = buildManager.ClickedArea.transform.GetChild(0).transform;
        Vector3 placeVec = placement.position;
        Vector3 towerPlace = new Vector3(placeVec.x, placeVec.y, placeVec.z);

        preTower = Instantiate(tower, towerPlace, placement.rotation);
        preTower.name = "IHAteMyLife";
        GameObject radius = preTower.transform.GetChild(0).gameObject;
        
        preTower.layer = 12;
        preTower.GetComponent<Renderer>().material.color = towerPreview;

        Tower tow = tower.GetComponent<Tower>();       
/*        radius.transform.localScale = new Vector3(tow.range * 2f, 0.01f, tow.range * 2f);*/
        radius.SetActive(true);
    }

    public void DestroyPreTower()
    {
        Destroy(preTower);
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
                placementHit = hit.collider.gameObject;
                if (placementHit.CompareTag("PlaceForTower"))
                {
                    buildManager.ClickedArea = _selection.gameObject;
                    buildPanel.transform.position = buildManager.ClickedArea.transform.position;
                    hintsPanel.transform.position = buildManager.ClickedArea.transform.position;

                    cursorTransform.gameObject.SetActive(false);

                    placementHit.layer = LayerMask.NameToLayer("Ignore Raycast");

                    buildPanel.SetActive(true);
                    hintsPanel.SetActive(true);
                    stopHover = true;
                    stopMouse = true;
                    placementClicked = true;

                }
            }
        }
        
    }
    void ClickedTower()
    {
        RaycastHit hit = CastRayFromCamera(towerLayerMask);
        if (hit.collider != null)
        {
            towerHit = hit.collider.gameObject;

            if (towerHit != null && preTower == null)
            {
                selectedTower = towerHit.GetComponent<Tower>();
                towerHit.layer = LayerMask.NameToLayer("Ignore Raycast");
                selectedTower.ShowUpgradeUI(towerMenu);

                cursorTransform.gameObject.SetActive(false);

                EventHandler.Instance.InvokeEvent(new TowerClickedEvent("Tower Is clicked", selectedTower.gameObject));
                tankUpgrade.transform.position = towerHit.transform.position;
                hintsPanel.transform.position = towerHit.transform.position;
                hintsPanel.SetActive(true);
                buildManager.TowerToBuild = null;
                stopHover = true;
                stopMouse = true;
                placementClicked = true;
            }                     
        }
    }

	public void DeleteTower()
	{
		if (selectedTower != null)
		{
			selectedTower.GetComponent<Tower>().towerPlacement.layer = 10;
			Destroy(selectedTower.gameObject);
			Deselect();
		}
	}

	void ClickedGarage()
    {
        RaycastHit hit = CastRayFromCamera(garageLayer);
        if (hit.collider != null)
        {
            garageHit = hit.collider.gameObject;
            garageHit.layer = LayerMask.NameToLayer("Ignore Raycast");
            
            tankUpgrade.transform.position = garageHit.transform.position;
            hintsPanel.transform.position = garageHit.transform.position;

            cursorTransform.gameObject.SetActive(false);
            tankUpgrade.SetActive(true);
            hintsPanel.SetActive(true);

            stopHover = true;
            stopMouse = true;
            placementClicked = true;
        }
    }
}
