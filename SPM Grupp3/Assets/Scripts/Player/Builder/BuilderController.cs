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
    [SerializeField] private GameObject playerCursor;

    private Color player1Color;
    private Color player2Color;

    private Color startColor;
    private Transform _selection;
    private Transform towerSelection;
    private Transform garageSelection;
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
    private readonly bool previousYState;
    private GameObject preTower;
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

    [System.NonSerialized] public bool purchasedTower = false;

    void Start()
    {
        GameObject placement = GameObject.Find("PlaceForTower");
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
        playerUI.gameObject.SetActive(true);
        buildManager = GetComponentInParent<BuildManager>();
        EventHandler.RegisterListener<BoughtInUIEvent>(SetBoughtInUI);

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
        playerCursor.SetActive(true);
        SetCursorColor(playerCursor);
        cursorTransform = playerCursor.GetComponent<RectTransform>();
        //cursorTransform.gameObject.GetComponent<CursorHandler>().ShowCursor();
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
            ClickedTower();
            ClickedGarage();
            if (!stopMouse)
            {
                ClickedPlacement();
            }
            
            EventHandler.InvokeEvent(new UIClickedEvent(
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
            EventHandler.InvokeEvent(new PlayerSwitchEvent(
                description: "Player switched mode",
                playerContainer: transform.parent.gameObject
            ));
        }
    }

    public void Deselect()
    {
        buildManager.TowerToBuild = null;
        Destroy(preTower);
        if (_selection != null)
        {
            Renderer selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material.color = startColor;
        }

        if (placementHit != null && placementHit.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            placementHit.layer = LayerMask.NameToLayer("PlaceForTower");
        }
        else if (towerHit != null && towerHit.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            towerHit.layer = LayerMask.NameToLayer("Towers");
        }
        else if (garageHit != null && garageHit.layer == LayerMask.NameToLayer("Ignore Raycast"))
        {
            garageHit.layer = LayerMask.NameToLayer("Garage");
        }

        if (selectedTower != null)
        {
            selectedTower.Radius.SetActive(false);
        }
        purchasedTower = false;

        hintsPanel.GetComponent<Animator>().SetTrigger("Disappear");
        cursorTransform.gameObject.SetActive(true);
        //cursorTransform.gameObject.GetComponent<CursorHandler>().ShowCursor();

        for (int i = 0; i < towerMenu.childCount; i++)
        {
            towerMenu.GetChild(i).gameObject.SetActive(false);
        }

        stopHover = false;
        stopMouse = false;
        placementClicked = false;
        /*        towerPanel.SetActive(true);*/
       
        foreach (FadeBehaviour fadeBehaviour in FindObjectsOfType<FadeBehaviour>())
        {
            fadeBehaviour.ResetFade();
        }
    }

    private void OnEnable()
    {
        InputSystem.onAfterUpdate += UpdateVirtualMouse;
        if (cursorTransform != null)
        {
            ResetCursorPosition();
            cursorTransform.gameObject.SetActive(true);
            //cursorTransform.gameObject.GetComponent<CursorHandler>().ShowCursor();
        }
    }

    void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateVirtualMouse;
        if (cursorTransform != null && virtualMouse != null)
        {
            cursorTransform.gameObject.SetActive(false);
            ResetCursorPosition();
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
                if (pointerAction.ReadValue<Vector2>().x > 0.2f || pointerAction.ReadValue<Vector2>().y > 0.2f || pointerAction.ReadValue<Vector2>().x < -0.2f || pointerAction.ReadValue<Vector2>().y < -0.2f)
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

    void TowerHover()
    {
        RaycastHit towerHover = CastRayFromCamera(towerLayerMask);
        Transform selection = null;

        if (towerHover.collider != null)
        {
            selection = towerHover.transform;
            selection.GetComponent<Tower>().ShowOptionHower();
            towerSelection = selection;
        }
      
        if (towerSelection != null && selection == null)
        {
            towerSelection.GetComponent<Tower>().HideOptionHower();
            towerSelection = null;
        }
    }

    void GarageHover()
    {
        RaycastHit garageHover = CastRayFromCamera(garageLayer);
        Transform selection = null;

        if (garageHover.collider != null)
        {
            selection = garageHover.transform;
            selection.GetComponent<GarageTrigger>().ShowIndicator();
            garageSelection = selection;
        }

        if (garageSelection != null && selection == null)
        {
            garageSelection.GetComponent<GarageTrigger>().CloseIndicator();
            garageSelection = null;
        }
    }

    void Hover(RaycastHit hit)
    {        
        TowerHover();
        GarageHover();

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

    public void GhostTower(GameObject towerToDisplay)
    {
        if (buildManager.ClickedArea == null)
        {
            return;
        }

        GameObject tower = towerToDisplay.transform.Find("Container").gameObject;
        Transform placement = buildManager.ClickedArea.transform.GetChild(0).transform;
        Vector3 placeVec = placement.position;
        Vector3 towerPlace = new Vector3(placeVec.x, placeVec.y, placeVec.z);

        preTower = Instantiate(tower, towerPlace, placement.rotation);
        GameObject radius = preTower.transform.Find("Radius").gameObject;
        preTower.layer = 12;
        preTower.transform.Find("Level1").GetComponent<Renderer>().material.color = towerPreview;

        Tower tow = towerToDisplay.GetComponent<Tower>();       
        radius.transform.localScale = new Vector3(tow.range * 2f, 0.01f, tow.range * 2f);
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
                    buildPanel.transform.position = buildManager.ClickedArea.transform.position + new Vector3(0f,0.2f,0f);
                    hintsPanel.transform.position = buildManager.ClickedArea.transform.position;

                    cursorTransform.gameObject.GetComponent<CursorHandler>().HideCursor();

                    placementHit.layer = LayerMask.NameToLayer("Ignore Raycast");

                    buildPanel.SetActive(true);
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

            if (towerHit != null && preTower == null && towerHit.GetComponent<Tower>())
            {
                selectedTower = towerHit.GetComponent<Tower>();
                towerHit.layer = LayerMask.NameToLayer("Ignore Raycast");
                selectedTower.ShowUpgradeUI(towerMenu);
                selectedTower.Radius.SetActive(true);

                cursorTransform.gameObject.GetComponent<CursorHandler>().HideCursor();

                EventHandler.InvokeEvent(new TowerClickedEvent("Tower Is clicked", selectedTower.gameObject));
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
			selectedTower.TowerPlacement.layer = LayerMask.NameToLayer("PlaceForTower");
            GameManager.Instance.RemovePlacedTower(selectedTower.gameObject);
            buildManager.PlayBuildingEffect(selectedTower.gameObject.transform);
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

            cursorTransform.gameObject.GetComponent<CursorHandler>().HideCursor();

            tankUpgrade.SetActive(true);
            hintsPanel.SetActive(true);

            stopHover = true;
            stopMouse = true;
            placementClicked = true;
        }
    }

    public void HideCursor()
    {
        cursorTransform.gameObject.GetComponent<CursorHandler>().HideCursor();
        stopHover = true;
        stopMouse = true;
        if (_selection != null)
        {
            Renderer selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material.color = startColor;
        }

        playerUI.gameObject.SetActive(false);
        if (preTower != null)
        {
            Destroy(preTower);
        }
    }
}
