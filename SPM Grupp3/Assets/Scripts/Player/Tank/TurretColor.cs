using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretColor : MonoBehaviour
{
    [SerializeField] private Renderer turretRenderer;
    [SerializeField] private Renderer hatRenderer;

    void Awake()
    {
        int playerIndex = GetComponentInParent<TankState>().PlayerInput.playerIndex;

        if (playerIndex == 0)
        {
            turretRenderer.material.color = GameManager.Instance.Player1Color;
            hatRenderer.material.color = GameManager.Instance.Player1Color;
        }
        else
        {
            turretRenderer.material.color = GameManager.Instance.Player2Color;
            hatRenderer.material.color = GameManager.Instance.Player2Color;
        }
    }
}
