using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flamethrower : MonoBehaviour
{
    public static float FireDamage;
    [SerializeField] private float damage;
    private TankState state;
    private PlayerInput playerInput;
    private InputAction fireAction;
    [SerializeField] private ParticleSystem fireParticles;


    void Start()
    {
        state = GetComponentInParent<TankState>();
        playerInput = state.PlayerInput;
        fireAction = playerInput.actions["Shoot"];
        FireDamage = damage;
    }

	// Update is called once per frame
	private void LateUpdate()
	{
        if(fireAction.IsPressed())
        {
            fireParticles.Play();
        }
        else
        {
            fireParticles.Stop();
        }
    }
}
