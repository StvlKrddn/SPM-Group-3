using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

/*
 * This class is based on this YouTube tutorial: https://youtu.be/Y3WNwl1ObC8
 */

public class BuilderController : MonoBehaviour
{
    [SerializeField] private float cursorSpeed = 1000f;
    [Space]
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private Transform canvas;
    [SerializeField] private LayerMask placeForTowerLayerMask;
    [SerializeField] private LayerMask towerLayerMask;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color startColor;

    private Renderer rend;
    private Transform _selection;
    public BuildManager buildManager;

    Mouse virtualMouse;
    PlayerInput playerInput;
    private Vector2 newPosition;
    Vector2 screenMiddle;
    bool previousMouseState;
    bool previousYState;
    public bool clickTimer = true;

    void Awake()
    {
        /*buildManager = BuildManager.instance;*/
        screenMiddle = new Vector2(Screen.width / 2, Screen.height / 2);

        //EventHandler.Instance.RegisterListener<EnterBuildModeEvent>(EnterBuildMode);

        playerInput = transform.parent.GetComponent<PlayerInput>();

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

    private void Update()
    {
        bool clicked = Gamepad.current.aButton.IsPressed();
        if (clicked)
        {
            if (clickTimer)
            {
                clickTimer = false;
                ClickedPlacement();
                ClickedTower();
                Invoke("ChangeBackTimer", 0.5f);
            }         
        }
    }

    private void OnEnable()
    {
        cursorTransform.gameObject.SetActive(true);
        ResetPosition();
        InputSystem.onAfterUpdate += UpdateVirtualMouse;
    }

    void OnDisable()
    {
        ResetPosition();
        if (cursorTransform != null) cursorTransform.gameObject.SetActive(false);
        InputSystem.onAfterUpdate -= UpdateVirtualMouse;
        //EventHandler.Instance.UnregisterListener<GarageEvent>(EnterBuildMode);
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

        newPosition = MoveMouse();
        UpdateCursorImage(newPosition);
        CheckIfClicked();

        RaycastHit hit = CastRayFromCamera(placeForTowerLayerMask);
        Hover(hit);
    }


    Vector2 MoveMouse()
    {
        Vector2 cursorMovement = Gamepad.current.leftStick.ReadValue();
        cursorMovement *= cursorSpeed * Time.unscaledDeltaTime;
        Vector2 newPosition = virtualMouse.position.ReadValue() + cursorMovement;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, cursorMovement);

        return newPosition;
    }

    bool CheckIfClicked()
    {
        bool isAcceptPressed = Gamepad.current.aButton.IsPressed();

        
        bool isYPressed = Gamepad.current.yButton.IsPressed();
        // If the button is not already pressed
        if (previousMouseState != isAcceptPressed)
        {
            virtualMouse.CopyState(out MouseState mouseState);
            mouseState.WithButton(MouseButton.Left, isAcceptPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = isAcceptPressed;
            return true;
           
        }
 //       print(isYPressed + " " + previousYState);
        if(isYPressed != previousYState)
        {
            print("kommer den hit");
            virtualMouse.CopyState(out MouseState mouseState);
            mouseState.WithButton(MouseButton.Left, isYPressed);
            InputState.Change(virtualMouse, mouseState);
            previousYState = isYPressed;
            return true;
        }

        return false;
    }
    
    void UpdateCursorImage(Vector2 newPosition)
    {
        if (cursorTransform != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect: canvas.GetComponent<RectTransform>(), 
                screenPoint: newPosition,
                cam: canvas.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, 
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
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition);
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
                if (buildManager.TowerToBuild != null)
                {
                    buildManager.ClickedArea = _selection.gameObject;
                    buildManager.InstantiateTower();
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
                Tower tower = towerHit.GetComponent<Tower>();

                print("Amount of clicks: ");
                if (tower.radius.activeInHierarchy)
                {
                    tower.radius.SetActive(false);
                    tower.upgradeUI.SetActive(false);
                        
                }
                else
                {
                    tower.radius.SetActive(true);
                    tower.upgradeUI.SetActive(true);

                } 
            }
        }
    }

    void ChangeBackTimer()
    {
        clickTimer = true;
    }
}
