using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ParticleSystemJobs;

public class Flamethrower : MonoBehaviour
{
    public int fireDamage = 25;
    private CapsuleCollider capsuleCollider;
    private TankState state;
    private PlayerInput playerInput;
    private InputAction fireAction;
    [SerializeField] private ParticleSystem fireParticles;


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
            fireParticles.Play();
            capsuleCollider.enabled = true;
        }
        else
        {
            capsuleCollider.enabled = false;
            fireParticles.Stop();
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
