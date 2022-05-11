using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ParticleSystemJobs;

public class Flamethrower : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private int fireDamage = 40;
    private TankState state;
    private PlayerInput playerInput;
    private InputAction fireAction;
    [SerializeField] private ParticleSystem particleSystem;


    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        state = GetComponentInParent<TankState>();
        playerInput = state.PlayerInput;
        fireAction = playerInput.actions["Shoot"];
    }

	// Update is called once per frame
	private void LateUpdate()
	{
        if(fireAction.IsPressed())
        {
            particleSystem.Play();
            capsuleCollider.enabled = true;
        }
        else
        {
            capsuleCollider.enabled = false;
            particleSystem.Stop();
        }
        

    }

	private void OnTriggerStay(Collider other)
	{
        if (other.GetComponent<EnemyController>())
        {
            other.GetComponent<EnemyController>().HitByFire(fireDamage * Time.fixedDeltaTime);
        }
		
	}
}
